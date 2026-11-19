from datetime import datetime, timezone

from fastapi import FastAPI, HTTPException
from fastapi.responses import HTMLResponse
from pydantic import BaseModel

app = FastAPI(title="MyappInfra Demo", version="1.0.0")

# In-memory store: resets whenever Lambda starts a new instance.
# Use DynamoDB or another database for anything you need to keep.
items: dict[int, dict] = {}


class Item(BaseModel):
    name: str
    price: float
    in_stock: bool = True


@app.get("/", response_class=HTMLResponse)
def home():
    # The Lambda Web Adapter checks this route before sending traffic.
    return """
    <html>
      <head><title>MyappInfra</title></head>
      <body style="font-family: sans-serif; max-width: 600px; margin: 40px auto;">
        <h1>Hello from FastAPI on AWS Lambda</h1>
        <p>Deployed by a CDK Pipeline, by Perry Tribolet.</p>
        <ul>
          <li><a href="/docs">Interactive API docs</a></li>
          <li><a href="/health">Health check</a></li>
          <li><a href="/hello?name=Perry">Say hello</a></li>
        </ul>
      </body>
    </html>
    """


@app.get("/health")
def health():
    return {"status": "ok", "time": datetime.now(timezone.utc).isoformat()}


@app.get("/hello")
def hello(name: str = "world"):
    return {"message": f"Hello, {name}!"}


@app.get("/items")
def list_items():
    return [{"id": item_id, **item} for item_id, item in items.items()]


@app.get("/items/{item_id}")
def get_item(item_id: int):
    if item_id not in items:
        raise HTTPException(status_code=404, detail="Item not found")
    return {"id": item_id, **items[item_id]}


@app.post("/items", status_code=201)
def create_item(item: Item):
    item_id = max(items, default=0) + 1
    items[item_id] = item.model_dump()
    return {"id": item_id, **items[item_id]}


@app.delete("/items/{item_id}", status_code=204)
def delete_item(item_id: int):
    if item_id not in items:
        raise HTTPException(status_code=404, detail="Item not found")
    del items[item_id]