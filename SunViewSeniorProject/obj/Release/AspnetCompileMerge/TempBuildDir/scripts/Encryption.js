
function aesEncryption() {
    //Grabs the username and password
    //debugger;
    var txtUserName = document.getElementById("uname").value.trim();
    var txtPassword = document.getElementById("upass").value.trim();
    if (txtUserName == "") {
        return false;
    }
    else if (txtPassword == "") {
        return false;
    }
    else { //Begin encryption

        //Generate AES Key and IV
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

        //Generate encrypted username based on 'key' and 'iv'
        var encryptedlogin = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtUserName), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

        //Store encrypted username in hiddenfield
        document.getElementById("enuname").innerHTML = encryptedlogin;

        //Generate encrypted password based on 'key' and 'iv'
        var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtPassword), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });

        //Store encrypted username in hiddnefield
        document.getElementById("enupass").innerHTML = encryptedpassword;


    }
};

function encryptUser() {
    var txtUserName = document.getElementById("uname").value.trim();
    if (txtUserName == "") {
        return false;
    }
    else { //Begin encryption

        //Generate AES Key and IV
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

        //Generate encrypted username based on 'key' and 'iv'
        var encryptedlogin = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtUserName), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

        //Store encrypted username in hiddenfield
        document.getElementById("enuname").innerHTML = encryptedlogin;

    }
};

function encryptPassword() {
    var txtPassword = document.getElementById("upass").value.trim();

    if (txtPassword == "") {
        return false;
    }
    else { //Begin encryption

        //Generate AES Key and IV
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

        //Generate encrypted password based on 'key' and 'iv'
        var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtPassword), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });

        //Store encrypted username in hiddnefield
        document.getElementById("enupass").innerHTML = encryptedpassword;


    }
};
