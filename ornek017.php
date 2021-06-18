<?php
/*
    Birden fazla dosya yükleme:
        * Dizi şeklinde çoklu dosya yükleme (resim[] alanı)
        * İki farklı dosya alanı kullanma (resim[] ve ozgecmis)
*/
if (!isset($_POST['ad'])) {
    // sayfa ilk defa gösteriliyor
?>
    <form action="" method="POST" enctype="multipart/form-data">
        Kullanıcı Adı: <input type="text" name="ad" value="ali"><br>
        Profil Resmi: <input type="file" name="resim[]" multiple><br>
        Özgeçmiş: <input type="file" name="ozgecmis"><br>
        <input type="submit" name="giris" value="Kaydet">
    </form>
<?php
} else {
    // form submit edilmiştir
    echo '<pre>';
    print_r($_POST);
    print_r($_FILES);
    // Yüklenen dosyayı ele alma
    if (isset($_FILES['resim'])) {
        if ($_FILES['resim']['error'] == UPLOAD_ERR_OK) {
            echo 'Yükleme başarılı';
            if (move_uploaded_file($_FILES['resim']['tmp_name'], 'resim/profil/' . $_POST['ad'] . '.' . pathinfo($_FILES['resim']['name'])['extension'])) {
                echo 'Profil resmi güncellendi...';
            } else {
                echo 'Profil resmi güncellenirken hata oluştu!';
            }
        } else if ($_FILES['resim']['error'] == UPLOAD_ERR_NO_FILE) {
            echo 'Profil resmi yüklenmemiş';
        }
    }
}
