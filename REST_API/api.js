const Joi = require('joi');
const express = require('express');
const app = express();
app.use(express.json());

const models = [
    { id: 1, name: "model1"},
    { id: 2, name: "model2"},
    { id: 3, name: "model3"},
]

// main window
app.get('/', (req, res) => {
    // res.send('Hello World');
    res.send(require('path').join(__dirname, "../temporary_files/simple_csharp_project/ConsoleApp1/ConsoleApp1/Program.cs"));
});

app.post('/api/model', (req, res) => {
    // shape of object    
    const schema = Joi.object({
        name: Joi.string().min(3).required()
    });
    const result = schema.validate(req.body);
    console.log(result);

    if (result.error) {
        res.status(400).send(result.error.details[0].message);
        return;
    }

    const model = {
        id: models.length + 1,
        name: req.body.name
    }
    models.push(model);
    // by convention
    res.send(model);
});

app.get('/api/model', (req, res) => {
    // let id = req.query.id;
    let model = models.find(c => c.id === parseInt(req.query.id));
    if (!model) return res.status(404).send("the model was not found");
    res.send(model);
});

const port = 9876;
app.listen(port, () => console.log(`Listening on port ${port}...`));
