angular.module('QuizApp', [])
    .controller('QuizCtrl', function ($scope, $http) {
        $scope.answered = false;
        $scope.title = "loading question...";
        $scope.options = [];
        $scope.correctAnswer = false;
        $scope.working = false;

        $scope.answer = function () {
            return $scope.correctAnswer ? 'correct' : 'incorrect';
        };

        $scope.nextQuestion = function () {
            $scope.working = true;
            $scope.answered = false;
            $scope.title = "loading question...";
            $scope.options = [];
  
            $http.get("/api/trivia").then(function successCallback(response) {
                $scope.options = response.data.options;
                $scope.title = response.data.title;
                $scope.answered = false;
                $scope.working = false;
            }, function errorCallback(response) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };

        $scope.sendAnswer = function (option) {
            $scope.working = true;
            $scope.answered = true;

            $http.post('/api/trivia', { 'questionId': option.questionId, 'optionId': option.id }).then(function successCallback(response) {
                $scope.correctAnswer = (response.data === true);
                $scope.working = false;
            },function errorCallback(response) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };
    });