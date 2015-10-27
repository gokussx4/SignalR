'use strict';
angular.module('yapp')
  .factory('AuthInterceptorService', function($q, $injector) {

    var authInterceptorServiceFactory = {};

    var _request = function(config) {

      config.headers = config.headers || {};
      var authData = $injector.get('localStorageService').get('authorizationData');
      if (authData) {
        config.headers.Authorization = 'Bearer ' + authData.token;
      }

      return config;
    }

    var _responseError = function(rejection) {
    if (rejection.status === 401) {
      $injector.get('$state').go('login');
    }
    return $q.reject(rejection);
  }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
  });
