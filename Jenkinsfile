pipeline {
    agent any
    environment {
       registry = "my-registry:55000"
    }
    stages { 
        stage('Build') {
            steps {
                dir ("backend"){
                sh """
                    image="${registry}/rest-back:ci-${env.BUILD_NUMBER}"
                    docker build -t \$image .

                    docker push \$image
                    """
                }
                dir ("frontend"){
                sh """
                    image="${registry}/rest-front:ci-${env.BUILD_NUMBER}"
                    docker build -t \$image .

                    docker push \$image
                    """
                }
            }
        }
        stage('Deploy Demo') {
            steps {
                dir ("deploy") {
                    sh '''
                    docker stack deploy -c demo.yml demo
                    docker service scale demo_backend=0
                    docker service scale demo_backend=1
                    '''
                }
            }
        }
    }
}
