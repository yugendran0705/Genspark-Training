const express = require('express');
const mongoose = require('mongoose');

const app = express();
const PORT = 3000;
const MONGO_URL = process.env.MONGO_URL || 'mongodb://localhost:27017/db';

mongoose.connect(MONGO_URL)
  .then(() => console.log('Connected to MongoDB'))
  .catch(err => console.error('MongoDB connection error:', err));

app.get('/', (req, res) => res.send('Hello from Node API'));

app.listen(PORT, () => console.log(`API running on http://localhost:${PORT}`));
