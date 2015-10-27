'use strict';

/**
 * @ngdoc function
 * @name yapp.factory:authService
 * @description
 * # authService
 * Authentication Service of yapp
 */
angular.module('yapp')
  .factory('AuthService', function($http, $q, localStorageService, serviceBaseAddress) {
    var authentication = {
      isAuth: false,
      userName : ''
    };
    var authServiceFactory = {
      saveRegistration: saveRegistration,
      login: login,
      logOut: logOut,
      fillAuthData: fillAuthData,
      authentication: authentication
    };

    return authServiceFactory;

    function saveRegistration (registration) {

      logOut();

      return $http.post(serviceBaseAddress + '/api/account/register', registration).then(function(response) {
        return response;
      });
    }

    /**
     *
     * @param {{userName:string,password:string}} loginData
     * @returns {*}
     */
    function login (loginData) {

      var data = 'grant_type=password&username=' + loginData.userName + '&password=' + loginData.password;

      return $http.post(serviceBaseAddress + '/token', data, {
        headers: {'Content-Type': 'application/x-www-form-urlencoded'
        }})
        .then(function(response) {
          localStorageService.set('authorizationData', {
            token: response.data.access_token,
            userName: loginData.userName
          });

          authentication.isAuth = true;
          authentication.userName = loginData.userName;
        }, function(response) {
          logOut();
          return $q.reject(response.data);
        });
    }

    function logOut () {
      localStorageService.remove('authorizationData');
      authentication.isAuth = false;
      authentication.userName = '';
    }

    function fillAuthData () {
      var authData = localStorageService.get('authorizationData');
      if (authData) {
        authentication.isAuth = true;
        authentication.userName = authData.userName;
      }
    }

  });
