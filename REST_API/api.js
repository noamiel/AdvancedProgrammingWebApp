const Joi = require('joi');
const express = require('express');
const app = express();
app.use(express.json());

const port = 9876;
app.listen(port, () => console.log(`Listening on port ${port}...`));
