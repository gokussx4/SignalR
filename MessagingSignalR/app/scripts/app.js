'use strict';

/**
 * @ngdoc overview
 * @name yapp
 * @description
 * # yapp
 *
 * Main module of the application.
 */
angular
  .module('yapp', [
    'ui.router',
    'ngAnimate',
    'LocalStorageModule',
    'ngScrollbar'
  ])
  .config(function($stateProvider, $urlRouterProvider, $httpProvider) {
    $urlRouterProvider.when('/dashboard', '/dashboard/overview');
    $urlRouterProvider.otherwise('/login');

    $stateProvider
      .state('base', {
        abstract: true,
        url: '',
        templateUrl: 'views/base.html'
      })
      .state('login', {
        url: '/login',
        parent: 'base',
        templateUrl: 'views/login.html',
        controller: 'LoginCtrl',
        controllerAs: 'vm'
      })
      .state('register', {
        url: '/register',
        parent: 'base',
        templateUrl: 'views/register.html',
        controller: 'RegisterController',
        controllerAs: 'vm'
      })
      .state('dashboard', {
        url: '/dashboard',
        parent: 'base',
        templateUrl: 'views/dashboard.html',
        controller: 'DashboardCtrl',
        controllerAs: 'vm'
      })
      .state('overview', {
        url: '/overview',
        parent: 'dashboard',
        templateUrl: 'views/dashboard/overview.html',
        controllerAs: 'vm'
      })
      .state('reports', {
        url: '/reports',
        parent: 'dashboard',
        templateUrl: 'views/dashboard/reports.html',
        controllerAs: 'vm'
      })
      .state('messages', {
        url: '/messages',
        parent: 'dashboard',
        templateUrl: 'views/dashboard/messages.html',
        controller: 'MessageController',
        controllerAs: 'vm'
      });

    $httpProvider.interceptors.push('AuthInterceptorService');

  }).value('$', window.jQuery)
  .constant('serviceBaseAddress', 'http://messagingSignalRAPI.azurewebsites.net');
//.constant('serviceBaseAddress', 'http://localhost:49273');
