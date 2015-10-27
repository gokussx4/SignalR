'use strict';

angular.module('yapp')
  .controller('RegisterController', function($state, $timeout, AuthService) {
    var vm = this;
    vm.savedSuccessfully = false;
    vm.message = '';

    vm.registration = {
      email: '',
      password: '',
      confirmPassword: ''
    };

    vm.register = function() {
      AuthService.saveRegistration(vm.registration).then(function() {
          vm.savedSuccessfully = true;
          vm.message = 'User has been registered successfully, you will be redicted to login page in 2 seconds.';
          startTimer();
        },
        function(response) {
          if (response.data.modelState) {
            var errors = [];
            for (var key in response.data.modelState) {
              for (var i = 0; i < response.data.modelState[key].length; i++) {
                errors.push(response.data.modelState[key][i]);
              }
            }
            vm.message = 'Failed to register user due to:' + errors.join(' ');
          } else {
            vm.message = response.data.message;
          }
        });
    };

    var startTimer = function() {
      var timer = $timeout(function() {
        $timeout.cancel(timer);
        $state.go('login');
      }, 2000);
    };

  });
