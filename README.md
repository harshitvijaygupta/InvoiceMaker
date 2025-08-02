Created a ASP.Net WebAPI for invoice generation which uses QuestPDF for generation and making it downloadable based on below request format.

**RQ Format**
{
  "id": 1,
  "client": {
    "name": "John Doe",
    "email": "john@example.com",
    "address": "123 Elm Street"
  },
  "invoiceDate": "2025-07-17T00:00:00",
  "taxRate": 0.1,
  "items": [
    {
      "description": "Design Work",
      "quantity": 2,
      "unitPrice": 500
    },
    {
      "description": "Consultation",
      "quantity": 1,
      "unitPrice": 300
    }
  ]
}
