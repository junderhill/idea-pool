# My Idea Pool - Codementor Assesment Project

## Overview
This repository contains my solution for the 'My Idea Pool' service project. Within this README file I have documented some of the design designs that I have made whilst developing my solution.

## Architecture
I decided that with this project being a fairly small project there was no benefit at this stage in introducing multiple projects to separate out the various aspects of the project. The pragmatic approach is to keep the solution as simple as possible until the requirements become such that additional complexity is required.

## Testing
The majority of the work on the project was test driven up until the last few features I implemented. This was due to time constraints and being away at the time when the project was due for submission. In an ideal world I would have test driven all aspects of the code base.

## JWT Authentication
The default HEADER key for JWT bearer tokens in ASP.Net Core WebAPI is `Authorization` with the token being prefixed with `Bearer`. The requirements for the project showed the use of `X-Access-Token` instead, therefore I had to implement some middleware which checks the header of all requests and if the `X-Access-Token` is found it adds the token to the expected `Authorization` header.

## Idea Averages
I have decided at this stage to calculate the average within the Idea entity. This approach has some pros and cons. 
The advantages of doing this ensure that if the data is changed anywhere else that the average that is used and displayed to the consumer of the API is always correct. It also allows me to unit test the calculation of the average. We can also easily change the implementation of the `average_score` property without the need to go and change existing data.
Some of the disadvantages include having to bring back all the ideas for a user before we can sort on the average. If there were a large number of ideas this may incur performance problems however there is little benefit in prematurely optimising the code. At this stage the advantages of doing it in this way out weight the disadvantages.
