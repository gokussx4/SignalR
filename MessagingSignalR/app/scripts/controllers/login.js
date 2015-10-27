'use strict';

/**
 * @ngdoc function
 * @name yapp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of yapp
 */
angular.module('yapp')
  .controller('LoginCtrl', function($state, AuthService) {

    var vm = this;
    vm.submit = function() {
      if (!vm.loginForm.$valid) {
        return;
      }
      vm.error = '';
      AuthService.login({userName: vm.email, password: vm.password}).then(function() {
        $state.go('dashboard');
      }, function(data) {
        vm.error = data.error + ': ' + data.error_description;
      });
    };

  });
