<?php
/*
    Çoklu dosya yüklemelerin ve yüklenecek dosya türlerinin kontrolü
*/
if (!isset($_POST['ad'])) {
    // sayfa ilk defa gösteriliyor
?>
    <form action="" method="POST" enctype="multipart/form-data">
        Kullanıcı Adı: <input type="text" name="ad" value="ali"><br>
        Profil Resmi: <input type="file" name="resim[]" accept="image/*" multiple><br>
        Özgeçmiş: <input type="file" name="ozgecmis"><br>
        <input type="submit" name="giris" value="Kaydet">
    </form>
<?php
} else {
    // form submit edilmiştir
    echo '<pre>';
    print_r($_POST);
    print_r($_FILES);
    // Yüklenen resimleri ele alma
    if (isset($_FILES['resim'])) {
        $adet = count($_FILES['resim']['error']);
        for($i=0; $i<$adet; $i++){
            if ($_FILES['resim']['error'][$i] == UPLOAD_ERR_OK) {
                if(substr($_FILES['resim']['type'][$i], 0, 6) != 'image/'){
                    echo 'Lütfen sadece resim yükleyin : '.$_FILES['resim']['name'][$i].'<br>';
                }else{
                    echo 'Yükleme başarılı<br>';
                    if (move_uploaded_file($_FILES['resim']['tmp_name'][$i], 'resim/profil/' . $_POST['ad'] . "-$i." . pathinfo($_FILES['resim']['name'][$i])['extension'])) {
                        echo 'Profil resmi güncellendi...<br>';
                    } else {
                        echo 'Profil resmi güncellenirken hata oluştu!';
                    }
                }
            } else {
                echo 'Profil resmi yüklemede hata oluştu : ' . $_FILES['resim']['error'][$i];
            }
        }
    }
    // Yüklenen özgeçmişi ele alma
    if (isset($_FILES['ozgecmis'])) {
        if ($_FILES['ozgecmis']['error'] == UPLOAD_ERR_OK) {
            if($_FILES['ozgecmis']['type'] != 'application/pdf'){
                echo "Sadece PDF yükleyin!";
            }
            elseif (move_uploaded_file($_FILES['ozgecmis']['tmp_name'], 'resim/ozgecmis/' . $_POST['ad'] . "." . pathinfo($_FILES['ozgecmis']['name'])['extension'])) {
                echo 'Özgeçmiş güncellendi...<br>';
            } else {
                echo 'Özgeçmiş güncellenirken hata oluştu!';
            }
        } else {
            echo 'Özgeçmiş yüklemede hata oluştu : ' . $_FILES['ozgecmis']['error'];
        }
    } else {
        echo 'Özgeçmiş yükleme zorunludur!';
    }
}
