using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

namespace Online_Store_Management
{
    public partial class Search : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)  // Sayfa başlatılırken yapılacaklar
        {
            Ana_Ktg_Yukle();
            Search_List.AutoGenerateColumns = true;
        }

        protected void Page_Load(object sender, EventArgs e)  // Sayfa yüklenirken yapılacaklar
        {
            Search_Info.Visible = false;

        }

        protected void Ana_Ktg_SelectedIndexChanged(object sender, EventArgs e)  // Ana kategorileri gösteren DropDownList üzerinde seçim yaptığımızda olacaklar
        {
            Alt_Ktg.Items.Clear();
            Alt_Ktg.Items.Insert(0, new ListItem(""));
            Default df = new Default();
            Search_Info.Visible = false;
            Search_Area.Text = "";
            if (df.Alt_Kategori_Var_mi(Ana_Ktg.SelectedItem.Value))
            {
                df.Alt_Kategoriler_Listele(Ana_Ktg.SelectedItem.Value, Alt_Ktg);
                Alt_Ktg.Visible = true;

            }

            else
            {
                GV_Goster(Ana_Ktg.SelectedItem.Value);
                Alt_Ktg.Visible = false;

            }
            if (df.Alt_Kategori_Var_mi(Ana_Ktg.SelectedItem.Value) && Alt_Ktg.SelectedIndex == 0)
            {
                GV_Goster(Ana_Ktg.SelectedValue);
            }
            if (Ana_Ktg.SelectedIndex == 0)
            {
                Ara_Dialog.Text = "";
            }
        }

        private void Ana_Ktg_Yukle()  // Ana kategorileri gösteren DropDownList nesnesini dolduran metod
        {
            Alt_Ktg.Visible = false;
            Ara_Dialog.Visible = false;
            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                try
                {
                    var Temel_Kategoriler = OSE.Cathegories.Where(ctgs => ctgs.Master_Id == null).Select(ctgs => ctgs.Name).ToList();
                    foreach (string Kategori in Temel_Kategoriler)
                    {
                        Ana_Ktg.Items.Add(Kategori);
                    }
                }
                catch (Exception)
                {
                    Search_List.DataSource = "";
                    Search_List.DataBind();
                    Ara_Dialog.ForeColor = Color.Red;
                    Ara_Dialog.Text = "Beklenmeyen bir hata oluştu";
                    Ara_Dialog.Visible = true;

                }
            }
        }

        private void GV_Goster(string ctg_name) // Seçlen kategorideki ürünleri sayfadaki GridView nesnesinde gösteren metod
        {

            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                try
                {

                    var Ctg_Id = OSE.Cathegories.Where(c => c.Name == ctg_name).Select(c => c.Id).SingleOrDefault(); // Adı girilen kategorinin ID si
                    var Prd = (from P in OSE.Products where P.Cathegory_Id == Ctg_Id select P).ToList();
                    if (Prd.Count() != 0)
                    {
                        Search_List.DataSource = Prd;
                        Search_List.DataBind();
                        Ara_Dialog.Visible = false;
                    }
                    else
                    {
                        Ara_Dialog.ForeColor = Color.Red;
                        Ara_Dialog.Text = "Bu kategoride ürün bulunamadı";
                        Search_List.DataSource = "";
                        Search_List.DataBind();
                        Ara_Dialog.Visible = true;
                    }
                }
                catch (Exception)
                {
                    Ara_Dialog.ForeColor = Color.Red;
                    Ara_Dialog.Text = "Başarısız Arama";
                    Ara_Dialog.Visible = true;
                }
            }
        }

        protected void Alt_Ktg_SelectedIndexChanged(object sender, EventArgs e) // Alt kategorileri gösteren DropDownList üzerinde seçim yaptığımızda olacaklar
        {

            GV_Goster(Alt_Ktg.SelectedItem.Value);
            if (Alt_Ktg.SelectedIndex == 0)
            {
                GV_Goster(Ana_Ktg.SelectedItem.Value);
            }
            Search_Info.Visible = false;
            Search_Area.Text = "";
        }

        protected void Search_Button_Click(object sender, EventArgs e)   // Ara butonuna tıkladığımızda olacaklar
        {
            if (Search_Area.Text != "") // Arama metin alanı boş mu kontrolü
            {
                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                    int urun_var_mi = OSE.Products.Where(P => P.Name == Search_Area.Text).Count(); // Veritabanında aranan isimde kaç ürün olduğu kontrol ediliyor

                    if (urun_var_mi != 0) // Aranan ürün mevcut mu kontrolü
                    {
                        try
                        {
                            var Aranan_Urun = (from P in OSE.Products where (P.Name == Search_Area.Text) select P).ToList();
                            Search_List.DataSource = Aranan_Urun;
                            Search_List.DataBind();
                        }
                        catch (Exception)
                        {
                            Search_List.DataSource = "";
                            Search_List.DataBind();
                            Ara_Dialog.ForeColor = Color.Red;
                            Ara_Dialog.Text = "Arama Başarısız Oldu";
                            Ara_Dialog.Visible = true;
                        }
                    }
                    else
                    {
                        Search_List.DataSource = "";
                        Search_List.DataBind();
                        Search_Info.ForeColor = Color.Red;
                        Search_Info.Text = "Aradığınız ürün bulunamadı !";
                        Search_Info.Visible = true;
                    }
                }
            }
            else
            {
                Search_List.DataSource = "";
                Search_List.DataBind();
                Search_Info.ForeColor = Color.Red;
                Search_Info.Text = "Lütfen bir ürün adı girin !";
                Search_Info.Visible = true;

            }
            Ana_Ktg.SelectedIndex = 0;
            Alt_Ktg.Visible = false;
            Ara_Dialog.Visible = false;
        }

        protected void Go_HomePage_Click(object sender, EventArgs e)  // Anasayfaya yönlendiren event
        {
            Response.Redirect("Default.aspx");
        }
    }
}