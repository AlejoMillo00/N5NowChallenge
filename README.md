# N5NowChallenge
Challenge for N5Now - Clean Architecture Permissions Api


# DOCKER
The project includes a docker compose file that contains all required services to run the project. This includes: The .NET Api service, elastic search, sql server and kafka / zookeeper. 

# ELASTIC SEARCH
Accessing http://localhost:9200 we can check the running instance of elasticsearch. Then we can access http://localhost:9200/permission/_search to check the registered records.

# KAFKA
From the Kafka container, using the bash, we can check for the produced messages and consume them within the same container terminal using the following command:
<code>kafka-console-consumer --bootstrap-server kafka:9092 --topic permission_topic --from-beginning</code>

The "permission_topic" is created automatically when the TopicService is called for the first time.

Thank you! :)
