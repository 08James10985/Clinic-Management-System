using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace NU_Clinic
{
    public partial class WarehouseStockForm : Form
    {
        // API-FIRST INTEGRATION: This form uses HttpClient ONLY
        // No direct database connection - all data comes from PHP REST API
        private const string API_BASE = "http://localhost/finalintegproject/ioms_web/api";

        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        private Label lblTitle, lblProductId, lblQuantity, lblStatus;
        private TextBox txtProductId, txtQuantity;
        private Button btnCheckStock, btnDeductStock, btnRefresh;
        private DataGridView dgvProducts;
        private Timer refreshTimer;
        private string selectedRowId = null;
        private bool isBusy = false; // prevents status override during actions

        public WarehouseStockForm()
        {
            InitializeComponent();
            SetupControls();
            this.Load += WarehouseStockForm_Load;
        }

        private void SetupControls()
        {
            this.Text = "Warehouse Stock Manager";
            this.Dock = DockStyle.Fill;
            this.BackColor = System.Drawing.Color.White;

            lblTitle = new Label()
            {
                Text = "Medical Supply Warehouse",
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                Dock = DockStyle.None,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(500, 30),
                ForeColor = System.Drawing.Color.FromArgb(31, 60, 102)
            };

            // ── Row 1: Product ID + Quantity ─────────────────────────
            lblProductId = new Label()
            {
                Text = "Product ID:",
                Location = new System.Drawing.Point(20, 58),
                Size = new System.Drawing.Size(75, 22),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            txtProductId = new TextBox()
            {
                Location = new System.Drawing.Point(96, 57),
                Size = new System.Drawing.Size(70, 22)
            };

            lblQuantity = new Label()
            {
                Text = "Quantity:",
                Location = new System.Drawing.Point(180, 58),
                Size = new System.Drawing.Size(65, 22),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            txtQuantity = new TextBox()
            {
                Location = new System.Drawing.Point(247, 57),
                Size = new System.Drawing.Size(70, 22)
            };

            // ── Row 2: Buttons ────────────────────────────────────────
            btnCheckStock = new Button()
            {
                Text = "Check Stock",
                Location = new System.Drawing.Point(20, 88),
                Size = new System.Drawing.Size(110, 28),
                BackColor = System.Drawing.Color.FromArgb(31, 60, 102),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCheckStock.FlatAppearance.BorderSize = 0;

            btnDeductStock = new Button()
            {
                Text = "Deduct Stock",
                Location = new System.Drawing.Point(138, 88),
                Size = new System.Drawing.Size(110, 28),
                BackColor = System.Drawing.Color.FromArgb(192, 0, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnDeductStock.FlatAppearance.BorderSize = 0;

            btnRefresh = new Button()
            {
                Text = "↺ Refresh",
                Location = new System.Drawing.Point(256, 88),
                Size = new System.Drawing.Size(85, 28),
                BackColor = System.Drawing.Color.FromArgb(0, 130, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;

            // ── Status label ──────────────────────────────────────────
            lblStatus = new Label()
            {
                Text = "Loading...",
                Location = new System.Drawing.Point(20, 124),
                Size = new System.Drawing.Size(900, 20),
                ForeColor = System.Drawing.Color.Blue
            };

            // ── DataGridView ──────────────────────────────────────────
            dgvProducts = new DataGridView()
            {
                Location = new System.Drawing.Point(20, 150),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                Size = new System.Drawing.Size(900, 380),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = System.Drawing.Color.FromArgb(240, 244, 250)
                }
            };

            // Wire events
            btnCheckStock.Click += BtnCheckStock_Click;
            btnDeductStock.Click += BtnDeductStock_Click;
            btnRefresh.Click += (s, e) => { isBusy = false; LoadProducts(); };

            dgvProducts.CellDoubleClick += DgvProducts_CellDoubleClick;
            dgvProducts.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                refreshTimer?.Stop();
                isBusy = true;

                var row = dgvProducts.Rows[e.RowIndex];
                selectedRowId = row.Cells["id"].Value.ToString();
                txtProductId.Text = selectedRowId;
                txtQuantity.Text = "1";

                lblStatus.Text = $"Selected: {row.Cells["name"].Value}  |  Stock: {row.Cells["stock"].Value}";
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(31, 60, 102);

                // ── Auto-trigger Check Stock so Deduct becomes enabled ──
                BtnCheckStock_Click(this, EventArgs.Empty);

                // Resume silent refresh after 15 seconds
                System.Threading.Tasks.Task.Delay(15000).ContinueWith(t =>
                {
                    this.Invoke((Action)(() =>
                    {
                        isBusy = false;
                        refreshTimer?.Start();
                    }));
                });
            };

            this.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblProductId, txtProductId,
                lblQuantity,  txtQuantity,
                btnCheckStock, btnDeductStock, btnRefresh,
                lblStatus,
                dgvProducts
            });
        }

        // ── LOAD ────────────────────────────────────────────────────
        private void WarehouseStockForm_Load(object sender, EventArgs e)
        {
            LoadProducts();

            refreshTimer = new Timer() { Interval = 5000 };
            refreshTimer.Tick += (s, ev) =>
            {
                if (!isBusy) // only refresh status if no action is happening
                    LoadProductsSilent();
            };
            refreshTimer.Start();
        }

        // Silent refresh — updates grid but does NOT touch lblStatus
        private async void LoadProductsSilent()
        {
            try
            {
                string json = await _http.GetStringAsync($"{API_BASE}/get_products.php");
                var obj = JObject.Parse(json);
                if ((bool)obj["success"])
                {
                    var products = obj["products"].ToObject<System.Data.DataTable>();
                    refreshTimer?.Stop();
                    dgvProducts.DataSource = products;
                    RestoreSelectedRow();
                    refreshTimer?.Start();
                }
            }
            catch { } // silent — don't override status
        }

        // Full load — shows status message
        private async void LoadProducts()
        {
            try
            {
                string json = await _http.GetStringAsync($"{API_BASE}/get_products.php");
                var obj = JObject.Parse(json);
                if ((bool)obj["success"])
                {
                    var products = obj["products"].ToObject<System.Data.DataTable>();
                    refreshTimer?.Stop();
                    dgvProducts.DataSource = products;
                    RestoreSelectedRow();
                    refreshTimer?.Start();

                    if (!isBusy)
                    {
                        lblStatus.Text = $"✅ Loaded {products.Rows.Count} products from warehouse.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
            catch
            {
                lblStatus.Text = "🔴 Cannot connect to warehouse. Is XAMPP Apache running?";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void RestoreSelectedRow()
        {
            if (selectedRowId == null) return;
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["id"].Value?.ToString() == selectedRowId)
                {
                    row.Selected = true;
                    dgvProducts.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        // ── DOUBLE CLICK ─────────────────────────────────────────────
        private void DgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            refreshTimer?.Stop();
            isBusy = true;

            var row = dgvProducts.Rows[e.RowIndex];
            selectedRowId = row.Cells["id"].Value.ToString();
            txtProductId.Text = selectedRowId;
            txtQuantity.Text = "1";

            lblStatus.Text = $"Selected: {row.Cells["name"].Value}  |  Stock: {row.Cells["stock"].Value}";
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(31, 60, 102);

            // Resume silent refresh after 15 seconds
            System.Threading.Tasks.Task.Delay(15000).ContinueWith(t =>
            {
                this.Invoke((Action)(() =>
                {
                    isBusy = false;
                    refreshTimer?.Start();
                }));
            });
        }

        // ── CHECK STOCK ──────────────────────────────────────────────
        private async void BtnCheckStock_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProductId.Text.Trim(), out int productId))
            {
                SetStatus("⚠️ Enter a valid numeric Product ID.", System.Drawing.Color.Orange);
                return;
            }

            isBusy = true;
            SetStatus("Checking stock...", System.Drawing.Color.Gray);

            try
            {
                string json = await _http.GetStringAsync($"{API_BASE}/check_stock.php?product_id={productId}");
                var obj = JObject.Parse(json);

                if ((bool)obj["success"])
                {
                    int stock = (int)obj["stock"];
                    bool avail = (bool)obj["available"];

                    btnDeductStock.Enabled = avail;

                    SetStatus(
                        avail
                            ? $"✅ {obj["name"]} — Stock: {stock} units. Available!"
                            : $"⚠️ {obj["name"]} — Out of stock!",
                        avail ? System.Drawing.Color.Green : System.Drawing.Color.Orange
                    );
                }
                else
                {
                    SetStatus("❌ " + obj["message"], System.Drawing.Color.Red);
                }
            }
            catch
            {
                SetStatus("🔴 Cannot connect to warehouse. Is XAMPP Apache running?", System.Drawing.Color.Red);
            }
        }

        // ── DEDUCT STOCK ─────────────────────────────────────────────
        private async void BtnDeductStock_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProductId.Text.Trim(), out int productId) ||
                !int.TryParse(txtQuantity.Text.Trim(), out int quantity) ||
                quantity <= 0)
            {
                SetStatus("⚠️ Enter valid Product ID and Quantity (> 0).", System.Drawing.Color.Orange);
                return;
            }

            isBusy = true;
            btnDeductStock.Enabled = false;
            SetStatus("Processing deduction...", System.Drawing.Color.Gray);

            try
            {
                var payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    product_id = productId,
                    quantity = quantity,
                    staff_id = 1
                });

                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{API_BASE}/deduct_stock.php", content);
                string json = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);

                bool success = (bool)obj["success"];

                SetStatus(
                    success
                        ? $"✅ {obj["message"]}  |  Order #{obj["order_id"]}  |  Remaining: {obj["remaining_stock"]} units"
                        : $"❌ {obj["message"]}",
                    success ? System.Drawing.Color.Green : System.Drawing.Color.Red
                );

                if (success)
                {
                    selectedRowId = null;
                    LoadProductsSilent(); // refresh grid quietly
                }
            }
            catch
            {
                SetStatus("🔴 Cannot connect to warehouse. Is XAMPP Apache running?", System.Drawing.Color.Red);
            }
        }

        // ── HELPER ───────────────────────────────────────────────────
        private void SetStatus(string text, System.Drawing.Color color)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }
    }
}