using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace NU_Clinic
{
    public partial class WarehouseStockForm : Form
    {
        private const string API_BASE = "http://localhost/finalintegproject/ioms_web/api";

        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        private Label lblTitle, lblProductId, lblQuantity, lblStatus;
        private TextBox txtProductId, txtQuantity;
        private Button btnCheckStock, btnDeductStock;

        private DataGridView dgvProducts;
        private Timer refreshTimer;

        private bool isUserInteracting = false;

        public WarehouseStockForm()
        {
            InitializeComponent();
            SetupControls();
            this.Load += WarehouseStockForm_Load;
        }

        private void SetupControls()
        {
            this.Text = "Warehouse Stock Manager";
            this.Size = new System.Drawing.Size(700, 500);

            lblTitle = new Label()
            {
                Text = "Medical Supply Warehouse",
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(400, 30)
            };

            lblProductId = new Label() { Text = "Product ID:", Location = new System.Drawing.Point(20, 70) };
            txtProductId = new TextBox() { Location = new System.Drawing.Point(110, 67), Size = new System.Drawing.Size(80, 20) };

            lblQuantity = new Label() { Text = "Quantity:", Location = new System.Drawing.Point(210, 70) };
            txtQuantity = new TextBox() { Location = new System.Drawing.Point(280, 67), Size = new System.Drawing.Size(80, 20) };

            btnCheckStock = new Button()
            {
                Text = "Check Stock",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(110, 30)
            };

            btnDeductStock = new Button()
            {
                Text = "Deduct Stock",
                Location = new System.Drawing.Point(140, 100),
                Size = new System.Drawing.Size(110, 30),
                Enabled = false
            };

            lblStatus = new Label()
            {
                Text = "Loading data...",
                Location = new System.Drawing.Point(20, 140),
                Size = new System.Drawing.Size(640, 20),
                ForeColor = System.Drawing.Color.Blue
            };

            dgvProducts = new DataGridView()
            {
                Location = new System.Drawing.Point(20, 170),
                Size = new System.Drawing.Size(640, 270),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Events
            btnCheckStock.Click += BtnCheckStock_Click;
            btnDeductStock.Click += BtnDeductStock_Click;

            dgvProducts.CellDoubleClick += DgvProducts_CellDoubleClick;

            dgvProducts.CellClick += (s, e) => isUserInteracting = true;
            dgvProducts.MouseEnter += (s, e) => isUserInteracting = true;
            dgvProducts.MouseLeave += (s, e) => isUserInteracting = false;

            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblProductId, txtProductId,
                lblQuantity, txtQuantity,
                btnCheckStock, btnDeductStock,
                lblStatus, dgvProducts
            });
        }

        private void WarehouseStockForm_Load(object sender, EventArgs e)
        {
            LoadProducts();

            refreshTimer = new Timer();
            refreshTimer.Interval = 3000;
            refreshTimer.Tick += (s, ev) =>
            {
                if (!isUserInteracting)
                    LoadProducts();
            };
            refreshTimer.Start();
        }

        private async void LoadProducts()
        {
            try
            {
                object selectedId = null;

                if (dgvProducts.CurrentRow != null &&
                    dgvProducts.CurrentRow.Cells["id"].Value != null)
                {
                    selectedId = dgvProducts.CurrentRow.Cells["id"].Value;
                }

                string json = await _http.GetStringAsync($"{API_BASE}/get_products.php");
                var obj = JObject.Parse(json);

                if ((bool)obj["success"])
                {
                    var products = obj["products"].ToObject<System.Data.DataTable>();
                    dgvProducts.DataSource = products;

                    // restore selected row
                    if (selectedId != null)
                    {
                        foreach (DataGridViewRow row in dgvProducts.Rows)
                        {
                            if (row.Cells["id"].Value.ToString() == selectedId.ToString())
                            {
                                row.Selected = true;
                                dgvProducts.CurrentCell = row.Cells[0];
                                break;
                            }
                        }
                    }

                    lblStatus.Text = $"Loaded {products.Rows.Count} products.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch
            {
                lblStatus.Text = "Cannot connect to warehouse.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void DgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvProducts.Rows[e.RowIndex];

                txtProductId.Text = row.Cells["id"].Value.ToString();
                txtQuantity.Text = "1";

                lblStatus.Text = $"Selected: {row.Cells["name"].Value}";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
            }
        }

        private async void BtnCheckStock_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProductId.Text.Trim(), out int productId))
            {
                lblStatus.Text = "Enter valid Product ID.";
                return;
            }

            try
            {
                string json = await _http.GetStringAsync($"{API_BASE}/check_stock.php?product_id={productId}");
                var obj = JObject.Parse(json);

                if ((bool)obj["success"])
                {
                    int stock = (int)obj["stock"];
                    bool avail = (bool)obj["available"];

                    lblStatus.Text = avail
                        ? $"{obj["name"]} — Stock: {stock}"
                        : $"{obj["name"]} — Out of stock";

                    lblStatus.ForeColor = avail
                        ? System.Drawing.Color.Green
                        : System.Drawing.Color.Orange;

                    btnDeductStock.Enabled = avail;
                }
            }
            catch
            {
                lblStatus.Text = "Cannot connect to warehouse.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private async void BtnDeductStock_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProductId.Text.Trim(), out int productId) ||
                !int.TryParse(txtQuantity.Text.Trim(), out int quantity) ||
                quantity <= 0)
            {
                lblStatus.Text = "Enter valid Product ID and Quantity.";
                return;
            }

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

                lblStatus.Text = (bool)obj["success"]
                    ? $"{obj["message"]} | Remaining: {obj["remaining_stock"]}"
                    : obj["message"].ToString();

                lblStatus.ForeColor = (bool)obj["success"]
                    ? System.Drawing.Color.Green
                    : System.Drawing.Color.Red;

                LoadProducts(); // instant refresh
            }
            catch
            {
                lblStatus.Text = "Cannot connect to warehouse.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}