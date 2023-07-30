# StockMonitoring

Repository for projects related to stock monitoring.

<br>

## PriceTracker

The aim of this project is to track a stock price along the time and warn the user if it eaches the desired buying or selling price.

<br>

### Data

This project uses the data provided by the https://brapi.dev/ API.

<br>

### Application Settings

#### API

Defines the data API's base URL for the `ApiService`.

```json
"API": {
    "Url": "https://brapi.dev/api"
}
```

<br>

#### SMTP

This configuration block holds the parameters used by the `EmailService` to send the e-mail alerts.

```json
"SMTP": {
    "Host": "",
    "Port": 0,
    "UserName": "",
    "Password": ""
}
```

<br>

#### Sender

This configuration block holds the parameters used by the `EmailService` to identify the e-mails sender.

```json
"Sender": {
    "Name": "",
    "Address": ""
}
```

<br>

#### Recipients

This configuration block holds the parameters used by the `EmailService` to identify the e-mails recipients.

```json
"Recipients": [
    {
        "Name": "",
        "Address": ""
    }
]
```

<br>

### Application Parameters

The parameters below must be passed to the program as command-line arguments.

| Param        | Type    | Description               | Required |
| -            | -       | -                         | -        |
| Ticker       | String  | The stock ticker.         | Yes      |
| SellingPrice | Decimal | The target selling price. | Yes      |
| BuyingPrce   | Decimal | The target buying price.  | Yes      |


<br>

### Run 

Example command:

```
> PriceTracker.exe PETR4 22.67 22.59
```

The application will start and run in the background.