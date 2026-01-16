# DovizKontrolUygulamasi

Döviz verilerini ödev de yazıldığı gibi Frankfurter FREE API üzerinden çektim
Gelen karmaşık JSON verisi içerisinden sadece ihtiyacımız olan para birimlerini ve değerlerini süzdüm.
Belirli bir miktardaki parayı, seçilen kur üzerinden hesaplayan matematiksel fonksiyonu yazdım.
Çektiğimiz verileri ileride analiz etmek üzere bir veri tabanına kaydettim.
Uygulamanın sürekli kapanmaması için belirli aralıklarla veriyi yenileyen bir döngü kurdum.
Proje dosyalarını; ana kod, ayarlar ve yardımcı fonksiyonlar olarak mantıklı klasörlere ayırdım.
İnternet bağlantısı kesilmesi veya API limitinin dolması gibi durumlara karşı "Try-Except" blokları oluşturdum.
API'den gelen döviz verilerini LINQ ile sorgulanabilecek bir List<Currency> veya IEnumerable koleksiyonuna dönüştürdüm.
