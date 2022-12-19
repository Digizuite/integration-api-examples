# Project Title

Digizuite AW External Action Registration

## Description

This projects contains two json collections for Digizuite API (Requires at least 5.6.0).
- The 'Digizuite DC 5.6 API postman collection.json' is our main API (documented here: https://digizuite.atlassian.net/wiki/spaces/DD/pages/3000406852/DC+5.6+API+Documentation)
- The 'New Digizuite Content Search API_5.6.1_.postman_collection' is for our new content search API which is only from 5.6.1 and can be **subject to change**.

## Getting Started

### Dependencies

Digizuite products should be installed:
- DAM
- MM

### Installing

Copy Digizuite DC *.* API postman collection.json  to a local station.

### Executing program

* Open Postman and import the collection.
* Configure variables:
- baseUrl;
- username;
- password.
* Run "Authentication -> LogOn" request, this request will authenticate the user and save accessKey to global variable. This accessKey is using for all requests.
* Execute request.

## Help

For any additional information please visit [our AW documentation page](https://digizuite.atlassian.net/wiki/spaces/DD/pages/3000406852/DC+5.6+API+Documentation)

## Authors
Digizuite team

## Version History

* 0.2
	* DC API collection for 5.6 version

## License

This project is licensed under the Digizuite License

## Acknowledgments
