'use strict';

angular.module('yapp')
  .controller('MessageController', function($rootScope, $timeout, MessageService) {
    var vm = this;
    vm.message = '';
    vm.messages = [];
    vm.userCount = 0;
    vm.userTyping = '';
    vm.userIsTyping = userIsTyping;
    vm.clear = clear;
    vm.send = send;

    MessageService.all().then(function(data) {
      vm.messages = data;
      $rootScope.$broadcast('rebuild:me', {rollToBottom: true});
      vm.hub = MessageService.hub('MessagingHub');
      vm.hub.on('sendMessage', function(message) {
        vm.messages.push(message);
        $rootScope.$broadcast('rebuild:me', {rollToBottom: true});
      });
      vm.hub.on('sendUserCount', function(count) {
        vm.userCount = count;
      });
      vm.hub.on('messagesCleared', function() {
        vm.messages.length = 0;
      });
      vm.hub.on('sendUserTyping', function(user) {
        vm.userTyping = user;
        $rootScope.$broadcast('rebuild:me', {rollToBottom: true});
      });
      vm.hub.start();
    });

    function clear() {
      MessageService.clear();
    }

    function send() {
      MessageService.send(vm.message);
      vm.message = '';
    }

    function userIsTyping() {
      if (!vm.userTyping) {
        vm.hub.invoke('userIsTyping');
        $timeout(function() {
          vm.userTyping = '';
          $rootScope.$broadcast('rebuild:me', {rollToBottom: true});
        }, 5000);
      }
    }
  });
