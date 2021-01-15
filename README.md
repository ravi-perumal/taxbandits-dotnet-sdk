

# TaxBandits .NET Sample SDK
This is a sample based on .NET Framework to show how to authenticate and handshake with TaxBandits API. This sample has an endpoint which returns the number of businesses created with TaxBandits API.

 
## Configuration
You need to signup with TaxBandits Sandbox Developer Console at https://sandbox.taxbandits.com to get the keys to run the SDK. See below for more directions. 

Open 'JWTAuth/App.config' to add your client secret, client id and user token.


### To get the sandbox keys:

Go to Sandbox Developer console:  https://sandbox.taxbandits.com.

Signup or signin to Sandbox

Navigate to Settings and then to API Credentials. Copy Client Id, Client Secret and User Token.

### The sandbox urls:

Sandbox Auth Server: https://testoauth.expressauth.net/v2/tbsauth

API Server: https://testapi.taxbandits.com/v1.6.1

Sandbox Application URL: https://testapp.taxbandits.com


## Set up Test data using the Application:
You can go to the Sandbox Application to add sample data. 

* Login into sandbox application using https://testapp.taxbandits.com
* Once you are on the dashboard, go to “Quicklinks” at the top right and click “Address Book”
* You can add sample business from here
* Now you can run the SDK to receive the number of businesses created with TaxBandits.



