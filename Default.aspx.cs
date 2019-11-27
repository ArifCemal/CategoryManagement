using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing;

namespace Online_Store_Management
{
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)   // Anasayfa başlatılırken yapılacak işlemler
        {

            Edit_Screen.Visible = false;
            C_Register.Visible = false;
            C_Edit_Reg.Visible = false;
            Tekrar_Yukle.Visible = false;
            Alt_Kategoriler.Visible = false;
            C_Sub_Reg.Visible = false;
            Edit_Menu.Visible = false;

            Reg_Info.ForeColor = Color.Purple;
            Ana_Kategoriler_Listele();
            Kategori_Bilgi.Visible = false;
            Tekrar_Yukle.Text = "Kategorileri güncel olarak görmek için buraya tıklayarak sayfayı yenileyin";

        }


        protected void Page_Load(object sender, EventArgs e)   // Anasayfa yüklenirken yapılacak işlemler
        {
            Kategori_Bilgi.Visible = false;
            Tekrar_Yukle.Visible = false;
            Mesaj_Bilgisi.Visible = false;
            if (Ana_Kategoriler.Items.Count == 1)   // Sayfa yüklenirken kategori bulunamadığında yapılacak işlemler
            {
                Kategori_Bilgi.ForeColor = Color.Red;
                Kategori_Bilgi.Text = "Henüz kategori yok lütfen ekleyin  ";
                Kategori_Bilgi.Visible = true;
            }
        }


        private void Ana_Kategoriler_Listele()    // Sayfadaki lk DropDownList nesnesine ana kategorileri listeleyen metod
        {
            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                try
                {
                    var Temel_Kategoriler = OSE.Cathegories.Where(ctgs => ctgs.Master_Id == null).Select(ctgs => ctgs.Name).ToList();
                    foreach (string Kategori in Temel_Kategoriler)
                    {
                        Ana_Kategoriler.Items.Add(Kategori);  // Ana kategoriler listeleniyor
                    }
                }
                catch (Exception)
                {
                    Kategori_Bilgi.ForeColor = Color.Red;
                    Kategori_Bilgi.Text="Beklenmeyen bir hata oluştu";
                    Kategori_Bilgi.Visible = true;
               
                }

            }

        }

        public void Alt_Kategoriler_Listele(string Master_Ctg, DropDownList DL)  // Sayfadaki ikinci DropDownList nesnesine alt kategorileri listeleyen metod
        {
            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                var IDsi = OSE.Cathegories.Where(ctgs => ctgs.Name == Master_Ctg).Select(ctgs => ctgs.Id).FirstOrDefault(); // Seçilen ana kategroinin idsi
                var Alt_Kategoriler = OSE.Cathegories.Where(ctgs => ctgs.Master_Id == IDsi).Select(ctgs => ctgs.Name).ToList(); // Bu Id nin alt kategorilerinin isimleri
                foreach (string Kategori in Alt_Kategoriler)
                {
                    DL.Items.Add(Kategori);  // Alt kategoriler listeleniyor
                }

            }

        }

        public bool Alt_Kategori_Var_mi(string Kategori_Ad)       // Bir ana kategorinin alt kategorileri olup olmadığını kontrol eden metod
        {

            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {

                var IDsi = OSE.Cathegories.Where(ctgs => ctgs.Name == Kategori_Ad).Select(ctgs => ctgs.Id).FirstOrDefault(); // Seçilen kategroinin idsi
                var Alt_Kategoriler = OSE.Cathegories.Where(ctgs => ctgs.Master_Id == IDsi).Count(); // Bu Id nin alt kategorilerinin isimleri
                if (Alt_Kategoriler != 0)
                {
                    return true;
                }

                else
                {

                    return false;
                }
            }



        }

        private string Aciklama_Getir(string Ctg)   // Kategori açıklamalarını anasayfadaki textarea nesnesine getiren metod
        {
            string Aciklama = "";
            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                try
                {
                    Aciklama = OSE.Cathegories.Where(c => c.Name == Ctg).Select(c => c.Explanation).SingleOrDefault(); // Kategori Açıklamaları

                    Ayrinti.InnerHtml = Aciklama;
                }
                catch (InvalidOperationException)
                {
                    Mesaj_Bilgisi.ForeColor = Color.Red;
                    Mesaj_Bilgisi.Visible = true;
                    Mesaj_Bilgisi.Text = "Kategori açıklaması gösterilemiyor, muhtemelen aynı isimde birden fazla kategori var !";
                }

            }
            return Aciklama;
        }



        protected void Ana_Kategoriler_SelectedIndexChanged(object sender, EventArgs e) // Ana kategorileri listeleyen DropDownList üzerinde kategori değiştirdiğimizde yapılacaklar
        {
            Edit_Screen.Visible = false;
            Alt_Kategori_Ekle.Visible = true;
            Alt_Kategoriler.Items.Clear();
            Alt_Kategoriler.Items.Insert(0, new ListItem(""));
            Alt_Kategoriler.SelectedIndex = 0;


            if (Aciklama_Getir(Ana_Kategoriler.SelectedItem.Value) == "")
            {
                Ayrinti.InnerText = "Bu kategoriyle ilgili açıklama yok";
            }
            else
            {
                Ayrinti.InnerText = Aciklama_Getir(Ana_Kategoriler.SelectedItem.Value);
            }
            Edit_Menu.Visible = true;

            if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) )
            {

                Alt_Kategoriler_Listele(Ana_Kategoriler.SelectedItem.Value, Alt_Kategoriler);
                Alt_Kategoriler.Visible = true;

            }
            else
            {

                Alt_Kategoriler.Visible = false;
            }




        }


        protected void Alt_Kategoriler_SelectedIndexChanged(object sender, EventArgs e)  // Alt kategorileri listeleyen DropDownList üzerinde kategori değiştirdiğimizde yapılacaklar
        {
            Edit_Screen.Visible = false;

            if (Aciklama_Getir(Alt_Kategoriler.SelectedItem.Value) == "") { Ayrinti.InnerText = "Bu kategoriyle ilgili açıklama yok"; }

            else
            {
                Ayrinti.InnerText = Aciklama_Getir(Alt_Kategoriler.SelectedItem.Value);
            }

            Alt_Kategori_Ekle.Visible = false;
        }

        protected void Sil_Click(object sender, EventArgs e)       // Kategori silme işlemi yapacak butonumuza tıklandığında olacaklar
        {
            if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value!="" )  // Kategorinin Alt kategorisi seçiliyse onu sil
            {
                Kategori_Sil(Alt_Kategoriler.SelectedItem.Value);
            }
            else if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value == "") // Kategorinin alt kategorisi seçili değilse ana kategoriyi sil
            {
                Kategori_Sil(Ana_Kategoriler.SelectedItem.Value);
            }
            else
            {
                Kategori_Sil(Ana_Kategoriler.SelectedItem.Value);
            }
        }

        private void Kategori_Sil(String Ktg_Ad) // Kategori silmemizi sağlayan metod
        {
            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                try
                {
                    var IDsi = OSE.Cathegories.Where(ctgs => ctgs.Name == Ktg_Ad).Select(ctgs => ctgs.Id).FirstOrDefault(); // Seçilen kategroinin idsi


                    if (Alt_Kategori_Var_mi(Ktg_Ad))
                    {
                        Alt_Kategorilerini_Sil(IDsi);
                    }
                    else
                    {
                        Cathegory CT = OSE.Cathegories.Where(c => c.Name == Ktg_Ad).SingleOrDefault();

                        OSE.DeleteObject(CT); // İlgili kategori silindi
                    }
                    OSE.SaveChanges();

                    Bilgi_Mesaj.ForeColor = System.Drawing.Color.Green;


                    Bilgi_Mesaj.Text = "İlgili kategori silindi";

                    Tekrar_Yukle.Visible = true;
                    Tekrar_Yukle.ForeColor = Color.Brown;

                }

                catch (ArgumentNullException)
                {
                    Bilgi_Mesaj.ForeColor = System.Drawing.Color.Red;
                    Bilgi_Mesaj.Text = "Kategori Silinemedi: Kategori veya Alt kategori alanını boş bıraktınız, ya da onlardan biri zaten silinmiş... ! ";
                }

                catch (OptimisticConcurrencyException ex)
                {
                    Bilgi_Mesaj.ForeColor = System.Drawing.Color.Red;
                    Bilgi_Mesaj.Text = "Kategori Silinemedi: "+ex.Message;
                }

                catch (Exception )
                {
                    Bilgi_Mesaj.ForeColor = System.Drawing.Color.Red;
                    Bilgi_Mesaj.Text = "Kategori Silinemedi: Muhtemelen bu kategoride hala ürünler var ! ";
                }
            }

        }

        private void Alt_Kategorilerini_Sil(int id)  // Alt kategori silmemizi sağlayan metod
        {

            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                try
                {
                    OSE.Cathegories.Where(s => s.Id == id).ToList().ForEach(OSE.Cathegories.DeleteObject);
                    OSE.SaveChanges();
                }
                catch
                {
                    Bilgi_Mesaj.Visible = true;
                    Bilgi_Mesaj.ForeColor = Color.Red;
                    Bilgi_Mesaj.Text = "Alt kategorileri silinemedi";
                }
            }

        }

        protected void Tekrar_Yukle_Click(object sender, EventArgs e) // Sayfayı yeniden yükleyen event
        {
            Page.Response.Redirect("Default.aspx");
        }

        protected void Duzenle_Click(object sender, EventArgs e)    // Kategori düzenleme ekranını açan ve mevcut kategori bilgilerini getiren event
        {

            Reg_Info.Visible = true;
            Reg_Info.Text = "Düzenleme Ekranı";
            Edit_Screen.Visible = true;
            C_Edit_Reg.Visible = true;
            C_Sub_Reg.Visible = false;
            C_Register.Visible = false;
            string Kategori_Ad = "";
            string Kategori_Aciklama;



            if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value!="" )
            {
                Kategori_Ad = Alt_Kategoriler.SelectedItem.Value;

            }
            else if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value == "")
            {
                Kategori_Ad = Ana_Kategoriler.SelectedItem.Value;
            }
            else
            {
                Kategori_Ad = Ana_Kategoriler.SelectedItem.Value;

            }

            
                Kategori_Aciklama = Aciklama_Getir(Kategori_Ad);
            

            C_Name_Text.Text = Kategori_Ad;
            C_Exp_Text.Text = Kategori_Aciklama;

        }

        protected void Alt_Kategori_Ekle_Click(object sender, EventArgs e) // Ana kategorilere alt kategori ekleme ekranını açan event
        {
            Reg_Info.Visible = true;
            Reg_Info.Text = "Alt Kategori Ekleme Ekranı";
            Edit_Screen.Visible = true;
            C_Sub_Reg.Visible = true;
            C_Edit_Reg.Visible = false;
            C_Register.Visible = false;
        }

        protected void Yeni_Kategori_Click(object sender, EventArgs e) //  Yeni kategori ekleme ekranını açan event
        {
            Reg_Info.Visible = true;
            Reg_Info.Text = "Yeni kategori ekleme ekranı";
            Edit_Screen.Visible = true;
            C_Sub_Reg.Visible = false;
            C_Edit_Reg.Visible = false;
            C_Register.Visible = true;

        }

        private bool Kayit_Alan_Bos_mu()   // Kategori ekleme ekranı kullanılırken bilgilerin eksik girilip girilmediğini denetleyen metod
        {
            if (C_Name_Text.Text == "" || C_Exp_Text.Text == "")
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        protected void C_Register_Click(object sender, EventArgs e)  // Yeni Kategori ekleyen event
        {

            if (Kayit_Alan_Bos_mu())
            {
                Reg_Info.ForeColor = System.Drawing.Color.Red;

                Reg_Info.Text = "Lütfen her iki alanı da doldurun !";

            }
            else
            {
                using (Online_StoreEnt OSE = new Online_StoreEnt())
                {
                    try
                    {
                        Cathegory Ct = new Cathegory()
                        {
                            Name = C_Name_Text.Text,
                            Explanation = C_Exp_Text.Text

                        };
                        OSE.Cathegories.AddObject(Ct);
                        OSE.SaveChanges();
                        
                        Reg_Info.ForeColor = Color.Green;
                        Reg_Info.Text = "Kategori eklendi !";
                        Tekrar_Yukle.ForeColor = Color.Blue;
                        Tekrar_Yukle.Visible = true;
                       
                    }
                    catch (Exception ex)
                    {
                        Reg_Info.ForeColor = Color.Red;
                        Reg_Info.Text = "Kategori eklenemedi: " + ex.Message;
                    }
                   
                   
                }
           
            }
        }

        protected void C_Edit_Reg_Click(object sender, EventArgs e) // Kategori güncellememizi sağlayan event
        {
            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
              
                try
                {
                    if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value !="" )
                    {
                       Cathegory Ctg = (from ctg in OSE.Cathegories where ctg.Name == Alt_Kategoriler.SelectedItem.Value select ctg).SingleOrDefault();
                       Ctg.Name = C_Name_Text.Text;
                       Ctg.Explanation = C_Exp_Text.Text;
                    }
                    else if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value == "")
                    {
                          Cathegory Ctg = (from ctg in OSE.Cathegories where ctg.Name == Ana_Kategoriler.SelectedItem.Value select ctg).SingleOrDefault();
                        Ctg.Name = C_Name_Text.Text;
                        Ctg.Explanation = C_Exp_Text.Text;
                    }
                        
                    else
                    {
                        Cathegory Ctg = (from ctg in OSE.Cathegories where ctg.Name == Ana_Kategoriler.SelectedItem.Value select ctg).SingleOrDefault();
                        Ctg.Name = C_Name_Text.Text;
                        Ctg.Explanation = C_Exp_Text.Text;
                    }
                   
                    OSE.SaveChanges();
                    Reg_Info.ForeColor = Color.Green;
                    Reg_Info.Text = "Kategori Güncellendi !";
                    Tekrar_Yukle.Visible = true;
                }

                catch (Exception ex)
                {
                    Reg_Info.ForeColor = Color.Red;
                    Reg_Info.Text = "Kategori Güncellenemedi: " + ex.Message;
                }

               
            }
        }

        protected void C_Sub_Reg_Click(object sender, EventArgs e) // Ana kategorilere alt kategori eklememizi sağlayan event
        {

            using (Online_StoreEnt OSE = new Online_StoreEnt())
            {
                var ID = OSE.Cathegories.Where(ct => ct.Name == Ana_Kategoriler.SelectedItem.Value).Select(ctgs => ctgs.Id).FirstOrDefault(); // Seçilen kategroinin idsi
                if (Kayit_Alan_Bos_mu())
                {
                    Reg_Info.ForeColor = System.Drawing.Color.Red;

                    Reg_Info.Text = "Lütfen her iki alanı da doldurun !";

                }
                else
                {
                    try
                    {
                        Cathegory Ct = new Cathegory()
                        {
                            Master_Id = ID,
                            Name = C_Name_Text.Text,
                            Explanation = C_Exp_Text.Text

                        };
                        OSE.Cathegories.AddObject(Ct);
                        OSE.SaveChanges();
                        Reg_Info.ForeColor = Color.Green;
                        Reg_Info.Text = "Alt kategori seçili ana kategoriye eklendi !";
                        Tekrar_Yukle.Visible = true;

                    }
                    catch (Exception ex)
                    {
                        Reg_Info.ForeColor = Color.Red;
                        Reg_Info.Text = "Kategori eklenemedi: " + ex.Message;
                    }
                }
            }

        }

        protected void Urunler_Link_Click(object sender, EventArgs e) // Ürün düzenleme sayfasına yönlendiren event
        {
            Response.Redirect("Products.aspx");
        }

        protected void Search_Link_Click(object sender, EventArgs e) // Arama sayfasına yönlendiren event
        {
            Response.Redirect("Search.aspx");
        }



        protected void Urunlere_Git_Click1(object sender, EventArgs e) // Bizi ürün düzenleme sayfasına yönlendiren event (Ayrıca session kullanarak  anasayfadaki seçili kategori adını kaydediyor)
        {

            if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value) && Alt_Kategoriler.SelectedItem.Value!="" )
            {
                Session["Secili_Ad"] = Alt_Kategoriler.SelectedItem.Value;
            }
            else if (Alt_Kategori_Var_mi(Ana_Kategoriler.SelectedItem.Value)&& Alt_Kategoriler.SelectedItem.Value=="")
            {
                Session["Secili_Ad"] = Ana_Kategoriler.SelectedItem.Value;
            }
            else
            {
                Session["Secili_Ad"] = Ana_Kategoriler.SelectedItem.Value;
            }

            Response.Redirect("Products.aspx");
        }


    }

}