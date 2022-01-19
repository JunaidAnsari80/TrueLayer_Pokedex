
# TrueLayer_Pokedex


## The task


We would like you to develop your own fun 'Podedex' in the form of a REST API that returns Pokemon information. Don't worry, you will be using existing public APIs to do the heavy lifting.

The API has tow main endpoints:
1. Return basic Pokemon information
2. Return basic information but with 'fun' translation for Pokemon description


# Implementation
## Dependencies

 - .NET Core
 - NewtonSoft
 - Swashbuckle


## How to run.
Hit F5 to run. You will be presented API documentation.
https://localhost:44367/swagger/index.html


## Things do for production.

 - Implement rate limiter. Possibly using a API Gateway to align with 3rd party SLA for number of requests.
 - Use some thing like Polly to manage exception and HttpStatus condition rather than implementing try catch block.
 - Implement logs for each service. 


