angular.module('myApp', ['ngMaterial'])
    .controller("RouteCtrl", function($scope, $http){

        /**********************************************************
                           Ben's Functions
        Authetication functions:
        Checks if the user has logged in, on login stay on current page
        Else redirects to login page.

        Logout function will redirect user to login page and remove
        their IP address from the data base.
        **********************************************************/
        $scope.logout = function () {
            $http.post('codebehind.aspx/logout', {})
                .success(function (data, status) {
                    window.location.href = "index.html";
                })
                .error(function (status) {
                });
        };

        $scope.checkIP = function () {

            $http.post('codebehind.aspx/checkIP', {})
                .success(function (data, status) {
                    if (data.d == "false") {
                        $scope.logout();
                        //window.location.href = "index.html";
                    }
                })
                .error(function (data, status) {
                });
        }

        $scope.checkIP();

    })
    .controller("TestCtrl", function ($scope, $http, $mdDialog) {
        $scope.myStatus = "";
        $scope.inputError = "";

        /*********************************************************
        Loads the names of all the projects in the XML file into pnames.
        pnames is the options for prName. Populates the dropdownlist.
        **********************************************************/
        $scope.getProjects = function () {
            $scope.myStatus = "";
            $http.post('codebehind.aspx/GetAllProjects', { data: {} })
                .success(function (data, status) {
                    $scope.pnames = data.d;
                }).error(function (data, status) {
                    $scope.myStatus = "getproj";
                });
        };
        /**********************************************************
        Runs getprojects after every change. Keeps updated list of project names.
        **********************************************************/
        $scope.getProjects();

        /*********************************************************
        Saves the current project configuration to the XML file. 
        Tries to save all data, if it fails(no field selected) then uses
        second httprequest to save module name and cg ID.
        *********************************************************/
        $scope.saveConfig = function () {
            $http.post('codebehind.aspx/SaveProject', {
                pName: $scope.prName, mName: $scope.mName, cgID: $scope.cgID, fName: $scope.fNames, tfsName: $scope.tfsName, cgName: $scope.cgName,
                dir: $scope.dir, tfsVal: $scope.tfsVal, cgVal: $scope.cgVal, type: $scope.type, max: $scope.max
            })
            .success(function (data, status) {
                $scope.myStatus = "Saved!";
            }).error(function (data, status) {
                $http.post('codebehind.aspx/SaveProject2', {
                    pName: $scope.prName, mName: $scope.mName, cgID: $scope.cgID
                })
                .success(function (data, status) {
                    //$scope.myStatus = "secondSave";
                    $scope.myStatus = "Saved!";
                }).error(function (data, status) {
                    $scope.myStatus = "savep2";
                });
            });
            // }
        };

        /********************************************************************
        Populates the field dropdownlist with the map numbers. Populates the
        CGID textbox with the obtained CGID.
        Calls getMappings at end to repopulate field variables(in case of project
        name switch while on a specific map number).
        ********************************************************************/
        $scope.getNumbers = function (proj) {
            $scope.inputError = "";
            $scope.myStatus = "";
            $scope.prName = proj;
            $http.post('codebehind.aspx/GetNumberMappings', { projects: proj })
                .success(function (data, status) {
                    $scope.fiNames = data.d;
                }).error(function (data, status) {
                    $scope.myStatus = "getNumbers";
                });
            $http.post('codebehind.aspx/GetCGID', { projects: proj })
                .success(function (data, status) {
                    $scope.cgID = data.d[1];
                    $scope.mName = data.d[0];
                }).error(function (data, status) {
                    $scope.myStatus = "getCGID";
                });
            $scope.getMappings($scope.fNames);
        };
        /*****************************************************************
        Populates all remaining fields with specific map related information.
        ******************************************************************/
        $scope.getMappings = function (map) {
            $scope.myStatus = "";
            $scope.fNames = map;
            $http.post('codebehind.aspx/GetAllMappings', { projects: $scope.prName, number: map })
            .success(function (data, status) {
                $scope.tfsName = data.d[0];
                $scope.cgName = data.d[1];
                $scope.tfsVal = data.d[2];
                $scope.cgVal = data.d[3];
                $scope.max = data.d[4];
                $scope.dir = data.d[5];
                $scope.type = data.d[6];
            }).error(function (data, status) {
                //$scope.myStatus = "getMaps";
            });
        };
        /*******************************************************************
        Creates a new project with given new project name. Then calls getProjects()
        to refresh project names dropdownlist.
        ********************************************************************/
        $scope.projCreate = function (newName) {
            $scope.myStatus = "";
            if (newName != "") {
                for (var i = 0; i < $scope.pnames.length; i++) {
                    if (newName == $scope.pnames[i]) {
                        $scope.inputError = "Project name already exists";
                        return;
                    }
                }
                $http.post('codebehind.aspx/CreateProject', { name: newName })
                .success(function (data, status) {
                    $scope.getProjects();
                }).error(function (data, status) {
                    $scope.myStatus = "create project failed";
                });
            };
        };
        /****************************************************************************
        Deletes currently selected project
        *****************************************************************************/
        $scope.projDelete = function () {
            $scope.myStatus = "";
            if ($scope.pnames.length > 1) {
                $http.post('codebehind.aspx/DeleteProject', { pName: $scope.prName })
                    .success(function (data, status) {
                        $scope.myStatus2 = "success";
                        $scope.getProjects();
                    }).error(function (data, status) {
                        $scope.myStatus2 = "deleteP";
                    });
            }
            else {
                $scope.inputError = "There must be at least 1 project";
            }
        };

        /***************************************************************************
        Creates new mapping field with no attributes for current project
        ***************************************************************************/
        $scope.mapCreate = function () {
            $scope.myStatus = "";
            $http.post('codebehind.aspx/CreateMap', { pName: $scope.prName, fNames: $scope.fiNames })
                .success(function (data, status) {
                    $scope.myStatus = "map created";
                    $scope.getNumbers($scope.prName);
                }).error(function (data, status) {
                    $scope.myStatus = "map failed";
                });
        };

        /****************************************************************************
        Deletes currently selected mapping field
        ****************************************************************************/
        $scope.mapDelete = function () {
            $scope.myStatus = "";
            if ($scope.fiNames.length > 1) {
                $http.post('codebehind.aspx/DeleteMap', { pName: $scope.prName, fName: $scope.fNames })
                    .success(function (data, status) {
                        $scope.myStatus = "map deleted";
                        $scope.getNumbers($scope.prName);
                    }).error(function (data, status) {
                        $scope.myStatus = "map delete failed";
                    });
            }
            else {
                $scope.inputError = "There must be at least one mapping";
            }
        };
        /***************************************************************************
        Pop-up prompt box for new project name
        ***************************************************************************/
        $scope.showPrompt = function (ev) {
            var confirm = $mdDialog.prompt()
                .title("Enter new project name")
                .placeholder("Project name")
                .ariaLabel("Project name")
                .targetEvent(ev)
                .ok('Confirm')
                .cancel('Cancel');
            $mdDialog.show(confirm).then(function (result) {
                $scope.projCreate(result);
            });
        };
    })
    .controller("UserCtrl", function ($scope, $http) {
        $scope.newStatus = {};
        $scope.Mismatch = "";
        $scope.notification = "";
        $scope.getInfo = function () {
            $scope.notification = "";
            $http.post('codebehind.aspx/GetUserInfo', { data: {} })
                .success(function (data, status) {
                    $scope.tfsUser = data.d[0];
                    $scope.tfsPass = data.d[1];
                    $scope.cgUser = data.d[2];
                    $scope.cgPass = data.d[3];
                    $scope.cgPath = data.d[4];
                }).error(function (data, status) {
                    $scope.newStatus = status;
                });
        };
        $scope.getInfo();
        $scope.saveUser = function () {
            $http.post('codebehind.aspx/saveUserInfo', {
                cguser: $scope.cgUser, cgpass: $scope.cgPass, tfsuser: $scope.tfsUser, tfspass: $scope.tfsPass, cgpath: $scope.cgPath
            })
            .success(function (data, status) {
                $scope.notification = "Saved!";
            }).error(function (data, status) {
                $scope.newStatus = status;
            });
        };
    })
    .controller("AppendCtrl", function ($scope, $http) {
        $scope.apStatus = "";
        $scope.getAppend = function () {
            $scope.apStatus = "";
            $http.post('codebehind.aspx/getAppend', { data: {} })
                .success(function (data, status) {
                    $scope.fileName = data.d[0];
                    $scope.overwrite = data.d[1];
                    $scope.fileSize = data.d[2];
                    $scope.backSize = data.d[3];
                    $scope.rootLevel = data.d[4];
                }).error(function (data, status) {
                    $scope.apStatus = status;
                });
        };
        $scope.getAppend();
        $scope.saveAppend = function () {
            $http.post('codebehind.aspx/saveAppend', {
                file: $scope.fileName, append: $scope.overwrite, size: $scope.fileSize, roll: $scope.backSize, level: $scope.rootLevel
            })
                .success(function (data, status) {
                    $scope.apStatus = "Saved!"
                }).error(function (data, status) {
                    $scope.apStatus = status;
                });

        };

    });