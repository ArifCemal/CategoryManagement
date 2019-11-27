using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Online_Store_Management
{
    public partial class Products : System.Web.UI.Page
    {
        string Secili_Kategori;

        protected void Page_Init(object sender, EventArgs e) // Ürün düzenleme sayfası başlarken yapılacaklar
        {
            Urun_Sec_Ekran.Visible = false;
            Urun_Ekle_Ekran.Visible = false;
            Urun_Kategorisi.Visible = false;
        }
        protected void Page_Load(object sender, EventArgs e) // Ürün düzenleme sayfası yüklenirken yapılacaklar
        {
            if (Secili_Kategori != "")                                  
            {
                try
                {
                    Urun_Form_Info.Visible = false;
                    Secili_Kategori = (string)(Session["Secili_Ad"]);   // Anasayfada kaydedilen ana kategori bilgisi session dan okunuyor.
                }
                catch (Exception)
                {
                    Response.Write("Bir Hata Oluştu !");
                }
            }

            else
            {
                Urun_Kategorisi.ForeColor = Color.Red;
                Urun_Kategorisi.Text = "Kategori seçilmedi";
                Urun_Kategorisi.Visible = true;

            }
            try                                                    //  Alınan kategori bilgisine göre sayfadaki GridView nesnesine ürünler listeleniyor.
            {

                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                    Urun_Kategorisi.Text = Secili_Kategori+ " Kategorisi";
                    Urun_Kategorisi.Visible = true;
                    var IDsi = OSE.Cathegories.Where(ctgs => ctgs.Name == Secili_Kategori).Select(ctgs => ctgs.Id).FirstOrDefault(); // Seçilen kategroinin idsi
                    var Ilgili_Urunuler = (from U in OSE.Products where U.Cathegory_Id == IDsi select U).ToList();
                    Urunler_Tablo.DataSource = Ilgili_Urunuler;
                    Urunler_Tablo.DataBind();
                }
                if (Urunler_Tablo.Rows.Count == 0)
                {
                    Urun_Kategorisi.Text = Urun_Kategorisi.Text + " henüz ürün yok";

                }
            }
            catch (Exception)
            {
                Response.Write("Ürünler listelenemiyor");
            }

        }

        protected void Urun_Ekle_Click(object sender, EventArgs e) // Ürün ekleme ekranını açan event
        {
            Urun_Sec_Ekran.Visible = false;
            Urun_Ekle_Ekran.Visible = true;
            Button_Urun_Ekle.Visible = true;
            Button_Urun_Guncelle.Visible = false;
        }

        protected void Urun_Sec_Sil_Click(object sender, EventArgs e) // Silmek için ürün seçme ekranını açan event
        {
            Urun_Sec_Ekran.Visible = true;
            Urun_Guncelle.Visible = false;
            Urun_Sil.Visible = true;
            Urun_Ekle_Ekran.Visible = false;
        }

        protected void Urun_Guncelle_Click(object sender, EventArgs e)  // Ürün güncelleme ekranını açan ve güncellenecek ürünün bilgilerini getiren event
        {
            if (Urun_No_Text.Text != "")
            {
                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                    int var_mi = OSE.Products.Where(p => p.Name == Urun_No_Text.Text).Select(p => p.Name).Count();
                    if (var_mi == 0)
                    {
                        Urun_Form_Info.ForeColor = Color.Red;
                        Urun_Form_Info.Text = "Bu ada sahip bir ürün mevcut değil";
                        Urun_Form_Info.Visible = true;
                    }
                    else
                    {
                        Urun_Ekle_Ekran.Visible = true;
                        Button_Urun_Ekle.Visible = false;
                        Button_Urun_Guncelle.Visible = true;
                        Text_Urun_Adi.Text = Urun_No_Text.Text;
                        Text_Urun_Fiyat.Text = OSE.Products.Where(p => p.Name == Urun_No_Text.Text).Select(p => p.Price).FirstOrDefault();
                        Text_Urun_KDV.Text = OSE.Products.Where(p => p.Name == Urun_No_Text.Text).Select(p => p.Tax_Rate).FirstOrDefault();
                    }

                }

               
            }
            else
            {
                Urun_Form_Info.ForeColor = Color.Red;
                Urun_Form_Info.Text = "Lütfen bir ürün adı girin";
                Urun_Form_Info.Visible = true;


            }
        }

        protected void Urun_Sil_Click(object sender, EventArgs e) // Ürün silme işlemini gerçekleştiren event
        {

            if (Urun_No_Text.Text != "")
            {

                Urun_Sec_Ekran.Visible = true;
                Urun_Sil.Visible = true;
                Urun_Guncelle.Visible = false;
             
                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                    
                    try
                    {
                    
                        Product PT = OSE.Products.Where(p=>p.Name==Urun_No_Text.Text).SingleOrDefault();

                        OSE.DeleteObject(PT); // İlgili ürün silindi

                        OSE.SaveChanges();

                        Urun_Form_Info.ForeColor = Color.Green;
                        Urun_Form_Info.Text = "Ürün Silindi !";
                        Urun_Form_Info.Visible = true;
                        Sayfa_Yinele.Visible = true;
                    }
                     catch(ArgumentNullException)
                    {
                        Urun_Form_Info.ForeColor = Color.Red;
                        Urun_Form_Info.Text = "İşlem başarısız: Hatalı bir ürün adı girmiş olabilirsiniz";
                        Urun_Form_Info.Visible = true;
                     }
                    catch (Exception ex)
                    {
                        Urun_Form_Info.ForeColor = Color.Red;
                        Urun_Form_Info.Text = "İşlem başarısız: "+ex.Message;
                        Urun_Form_Info.Visible = true;
                    }


                }



            }
            else
            {
                Urun_Form_Info.ForeColor = Color.Red;
                Urun_Form_Info.Text = "Lütfen bir ürün adı girin";
                Urun_Form_Info.Visible = true;


            }
        }

        protected void Urun_Sec_Guncelle_Click(object sender, EventArgs e) // Ürün güncelleme ekranını açan event
        {
            Urun_Sec_Ekran.Visible = true;
            Urun_Guncelle.Visible = true;
            Urun_Sil.Visible = false;
            Urun_Ekle_Ekran.Visible = false;
           

        }

        protected void Button_Urun_Ekle_Click(object sender, EventArgs e) // Yeni ürün eklememizi sağlayan event
        {
            if (Ekle_Formu_Bos_mu())
            {
                Urun_Form_Info.ForeColor = Color.Red;
                Urun_Form_Info.Text = "Lütfen tüm alanları doldurun";
                Urun_Form_Info.Visible = true;
            }
            else
            {
                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                    var IDsi = OSE.Cathegories.Where(ctgs => ctgs.Name == Secili_Kategori).Select(ctgs => ctgs.Id).FirstOrDefault(); // Seçilen kategroinin idsi
                    try
                    {
                        Product Pt = new Product() // Ürün bilgileri ekleniyor
                        {
                            Name = Text_Urun_Adi.Text,
                            Price = Text_Urun_Fiyat.Text,
                            Tax_Rate = Text_Urun_KDV.Text,
                            Cathegory_Id = IDsi


                        };
                        OSE.Products.AddObject(Pt);
                        OSE.SaveChanges();

                        Urun_Form_Info.ForeColor = Color.Green;
                        Urun_Form_Info.Text = "Ürün eklendi !";
                        Urun_Form_Info.Visible = true;
                        Sayfa_Yinele.Visible = true;

                    }
                    catch (Exception ex)
                    {

                        Urun_Form_Info.ForeColor = Color.Red;
                        Urun_Form_Info.Text = "Ürün eklenemedi: " + ex.Message;
                        Urun_Form_Info.Visible = true;
                    }


                }
            }
        }

        private bool Ekle_Formu_Bos_mu()  // Ürün ekleme işlemi için girilen bilgilerin eksik olup olmadığını denetleyen metod
        {
            if (Text_Urun_Adi.Text == "" || Text_Urun_Fiyat.Text == "" || Text_Urun_KDV.Text == "")
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        protected void Button_Urun_Guncelle_Click(object sender, EventArgs e) // Ürün bilgilerini güncellememizi sağlayan event
        {
            if (Ekle_Formu_Bos_mu())
            {
                Urun_Form_Info.ForeColor = Color.Red;
                Urun_Form_Info.Text = "Lütfen tüm alanları doldurun";
                Urun_Form_Info.Visible = true;
            }
            else
            {
                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                        try
                        {
                            var P_ID = OSE.Products.Where(pr => pr.Name == Text_Urun_Adi.Text).Select(pr => pr.Cathegory_Id).FirstOrDefault(); // Seçilen ürünün kategori idsi
                            Product P = (from pr in OSE.Products where pr.Name == Urun_No_Text.Text select pr).SingleOrDefault();
                            P.Name = Text_Urun_Adi.Text;
                            P.Price = Text_Urun_Fiyat.Text;
                            P.Tax_Rate = Text_Urun_KDV.Text;
                            OSE.SaveChanges();
                            Urun_Form_Info.ForeColor = Color.Green;
                            Urun_Form_Info.Text = "Güncellendi " ;
                            Urun_Form_Info.Visible = true;
                            Sayfa_Yinele.Visible = true;
                        }
                        catch (Exception)
                        {
                            Urun_Form_Info.ForeColor = Color.Red;
                            Urun_Form_Info.Text = "Ürün eklenemedi: Bir hata var ";
                            Urun_Form_Info.Visible = true;
                        }
                    }
               
            }
        }

        protected void Sayfa_Yinele_Click(object sender, EventArgs e)     //Sayfayı yeniden yükleyen event
        {
            Response.Redirect("Products.aspx");
        }

        protected void Arama_Sayfa_Git_Click(object sender, EventArgs e)  // Arama sayfasına yönlendiren event
        {
            Response.Redirect("Search.aspx");
        }

        protected void Basa_Don_Click(object sender, EventArgs e)        // Anasayfaya yönlendiren event
        {
            Response.Redirect("Default.aspx");
        }


    }
}