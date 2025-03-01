# Technical questions of assignment

1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add

    Answer: I've worked on the assignment for about totally 2 hours. If I could spend more time I would add the followings:
     - Automatic provider switching and ``rate-limiting`` for quote-APIs
     - Using ``Polly`` to avoid `back-pressures`, `retries`, and `circuit-breaking` approaches
     - Dynamic caching for storing and unfiroming results of provider  APIs
     - Exception handling middleware for catching all exceptions (Currently I'm using ErrorOr to handle occured errors)
     - Registering providers using reflection in a dynamic approach
     - Providers' `http-request-count` and metrics (`availability` and `observability` of the providers)
     - Some background workers could be written instead of hard coding, to detect the list of Crypto signs supported by each provider
     - APIs pottentialy could have been implemented by .Net7 minimal APIs
     - Integration Tests, E2E tests are still needed
2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it

    Answer: .Net7 has a lot of new interesting features like:
    - built-in RateLimiting
    - Grpc Transcoding
    - Open Tracing tools and metrics
    - Generic Math (C# 11)
    - List Patterns (C# 11)
        - e.g:

            ``` C#
            var list = new () { 6, 3, 4, 1, 2, 4 };
            if (list is [.., var number, 4])
            {
                Console.WriteLine(number);
                //Outputs: 2
            }
            ```

3. How would you track down a performance issue in production? Have you ever had to do this?

    Answer: Both legacy and modern systems can face performance issue on the production environment. Observable systems can find and alert their issues with a transparent and accesible manner. For exapmle a combination of Prometheues as a metrics database, ElasticSearch as log storage, Kibana as logging dashboard, Jaegar as trace storage and a integration of them with an AlertManager can easily find the Bottlenecks and hot pathes. There are `Golden signals` to be implemted through monitoring, tracing and logging.
    - Traffic
    - Error rate
    - Saturation
    - Latency

    So  if we monitor above parameters the performance issue can be detected in a simple form. (e.g: One service is facing high latency or increased response time, because the Redis I/O has pressure because of a Disk issue and all of these can be seen inside Spans, Logs and Alerts)

4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

    Answer: Currently I'm reading Bulding Microservices of Sam Newman with one my collegues. Some of key things that I faced are:
    - Microservices are kind of services that can change super fast (Cost of change is low, so the change is easy and is not bound to any other dependent problem) He mostly talks about change and the cost of change
    - He talks about the downsides of Distributed Monolith and the difference with Microservices

5. What do you think about this technical assessment?

    Answer: That was cool and simple

6. Please, describe yourself using JSON.

    Answer:

    ``` Json
    {
        "$schema": "No limitations, No Schemas",
        "$id": "0",
        "Name": "Arash Bahari",
        "Description": "Good Friend, Developer, Problem Solver, Always Eager To Optimize And Refactor",
        "Age": 29.9999999,
        "Profiles": [
            "https://github.com/devArashBahari",

        ],
        "ActiveLearner": true,
        "ActiveListener": true
    }
    ```
