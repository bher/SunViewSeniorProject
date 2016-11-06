<!--
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="not_Default.aspx.cs" Inherits="SunViewLogin.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <link rel="stylesheet"  type="text/css" href="css/style.css"/>

</head>

<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.5.6/angular.min.js"></script>
<script src="/scripts/aes.js" type="text/javascript"></script>
<script src="/scripts/Encryption.js" type="text/javascript"></script>

    <body>
        
        <div ng-app="myApp" ng-controller="myControl">

			<form >

                <img id="ChangeGearimg" src="/img/ChangeGear.png" />
                <div class="login">
				<div class="text">User ID:</div>

				<input class="txtbox" id="uname" type="text" ng-model="user.ID" onkeyup="this.value = this.value.toLowerCase();">

				</br>

				<div class="text">User Password: </div>

				<input class="txtbox" id ="upass" type="password" ng-model="user.password">

				</br></br>

				<button ng-click="submit()" ng-init="encrypt.user='Admin';encrypt.password='Pass'">Login</button>

				</br>

				<p ng-hide="true"><span id="enuname"></span></p>
				<p ng-hide="true"><span id="enupass"></span></p>
                </div>
			</form>
		</div>

		<script>
		    var app = angular.module('myApp', [])
		    app.controller('myControl', function ($scope, $http) {
		        $scope.submit = function () {
		            aesEncryption();

		            try {
		                var httpRequest = $http({
		                    method: "POST",
		                    url: "Default.aspx/checkUser",
		                    dataType: 'json',
		                    data: { user: document.getElementById("enuname").innerHTML, pass: document.getElementById("enupass").innerHTML },
		                    headers: {
		                        "Content-Type": "application/json"
		                    }
		                });
		                //if success
		                httpRequest.success(function (data, status) {
		                    if (data.d == "true") {
		                        window.location.href = "temp.html";
		                    }
		                    else {
		                        if ($scope.user.password != "")
		                            alert("Incorrect username or password");
		                        $scope.user.password = "";
		                    }
		                });
		                //if fail
		                httpRequest.error(function (data, status) {
		                    $scope.success = status;
		                });
		            }
		            catch (e) { }
		        };
		    });
		</script>	
    </body>
</html>
