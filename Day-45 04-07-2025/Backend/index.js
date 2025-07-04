// server.js
const express = require('express');
const Razorpay = require('razorpay');
const cors = require('cors');
const app = express();
const axios = require('axios');
app.use(express.json(), cors());

razor = new Razorpay({
    key_id:    '',
    key_secret:''
});


const KEY_ID     = '';
const KEY_SECRET = '';
const auth = Buffer
  .from(`${KEY_ID}:${KEY_SECRET}`)
  .toString('base64');
// Create order
app.post('/orders', async (req, res) => {
    const response = await axios.post(
        'https://api.razorpay.com/v1/orders',
        {
            amount:   req.body.amount, 
            currency: 'INR'
        },
        {
            headers: {
            'Content-Type':  'application/json',
            'Authorization': `Basic ${auth}`
            }
        }
    )
    res.json(response.data);
});

// Create v2 Standard Checkout preference
app.post('/preferences', async (req, res) => {
    
});

app.listen(3000, () => console.log('Listening on 3000'));
