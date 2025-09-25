using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MovieAppWinForms
{
    public partial class MainForm : Form
    {
        private MovieRepository repo;
        private string dataFile;

        public MainForm()
        {
            InitializeComponent();
            dataFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movies.xml");
            repo = new MovieRepository(dataFile);
            RefreshGrid();
        }

        private void RefreshGrid(string filter = "")
        {
            var list = repo.Movies;
            if (!string.IsNullOrWhiteSpace(filter))
                list = list.Where(m => m.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                       m.Genre.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            dgvMovies.DataSource = null;
            dgvMovies.DataSource = list;
            dgvMovies.Columns["PosterPath"].Visible = false;
            dgvMovies.Columns["Id"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var m = new Movie
            {
                Title = txtTitle.Text.Trim(),
                Genre = txtGenre.Text.Trim(),
                Year = int.TryParse(txtYear.Text.Trim(), out var y) ? y : 0,
                Rating = double.TryParse(txtRating.Text.Trim(), out var r) ? r : 0,
                PosterPath = txtPoster.Text.Trim()
            };
            repo.Add(m);
            ClearInputs();
            RefreshGrid();
        }

        private void btnChoosePoster_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPoster.Text = ofd.FileName;
                    try { pbPoster.Image = Image.FromFile(ofd.FileName); } catch { pbPoster.Image = null; }
                }
            }
        }

        private void dgvMovies_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMovies.CurrentRow == null) return;
            var m = dgvMovies.CurrentRow.DataBoundItem as Movie;
            if (m == null) return;
            txtTitle.Text = m.Title;
            txtGenre.Text = m.Genre;
            txtYear.Text = m.Year.ToString();
            txtRating.Text = m.Rating.ToString();
            txtPoster.Text = m.PosterPath;
            try { pbPoster.Image = !string.IsNullOrEmpty(m.PosterPath) ? Image.FromFile(m.PosterPath) : null; } catch { pbPoster.Image = null; }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMovies.CurrentRow == null) return;
            var m = dgvMovies.CurrentRow.DataBoundItem as Movie;
            if (m == null) return;
            if (MessageBox.Show("Deletar o filme selecionado?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                repo.Delete(m.Id);
                ClearInputs();
                RefreshGrid();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMovies.CurrentRow == null) return;
            var m = dgvMovies.CurrentRow.DataBoundItem as Movie;
            if (m == null) return;
            m.Title = txtTitle.Text.Trim();
            m.Genre = txtGenre.Text.Trim();
            m.Year = int.TryParse(txtYear.Text.Trim(), out var y) ? y : 0;
            m.Rating = double.TryParse(txtRating.Text.Trim(), out var r) ? r : 0;
            m.PosterPath = txtPoster.Text.Trim();
            repo.Update(m);
            RefreshGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshGrid(txtSearch.Text.Trim());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtTitle.Text = "";
            txtGenre.Text = "";
            txtYear.Text = "";
            txtRating.Text = "";
            txtPoster.Text = "";
            pbPoster.Image = null;
        }
    }
}
