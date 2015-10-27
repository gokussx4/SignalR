'use strict';

/**
 * @ngdoc function
 * @name yapp.factory:MessageService
 * @description
 * # MessageService
 * Component to interface between API and use within web app
 */
angular.module('yapp')
  .factory('MessageService', function($, $http, $rootScope, localStorageService, serviceBaseAddress) {
    var service = {
      all: all,
      clear: clear,
      send: send,
      hub: backendFactory
    }
    return service;

    function all() {
      return $http.get(serviceBaseAddress + '/api/message').then(function(response) {
        return response.data;
      }, function(response) {
        console.log('failed to get messages: ' + response.status);
        return response;
      });
    }

    function clear() {
      $http.delete(serviceBaseAddress + '/api/messages/clear');
    }

    function send(message) {
      $http.post(serviceBaseAddress + '/api/message', {'content': message, 'when': new Date()});
    }

    function backendFactory(hubName) {
      var connection = $.hubConnection(serviceBaseAddress);
      connection.logging = true;
      connection.qs = {'access_token': localStorageService.get('authorizationData').token};
      var proxy = connection.createHubProxy(hubName);

      return {
        on: function(eventName, callback) {
          proxy.on(eventName, function(result) {
            $rootScope.$apply(function() {
              if (callback) {
                callback(result);
              }
            });
          });
        },
        invoke: function(methodName, callback) {
          proxy.invoke(methodName)
            .done(function(result) {
              if (callback) {
                callback(result);
              }
            });
        },
        start: function() {
          connection.start().done(function() {
            console.log('Now connected, connection ID=' + connection.id);
          });
        }
      };
    }
  });
