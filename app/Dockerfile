# 1. Start from a slim official Python image (pulled from AWS's public registry to avoid Docker Hub rate limits)
FROM public.ecr.aws/docker/library/python:3.12-slim

# 2. Add the AWS Lambda Web Adapter, which translates Lambda events into normal HTTP requests for uvicorn
COPY --from=public.ecr.aws/awsguru/aws-lambda-adapter:1.0.1 /lambda-adapter /opt/extensions/lambda-adapter

# 3. Tell the adapter which port your app listens on
ENV PORT=8080

# 4. Work inside /app in the container
WORKDIR /app

# 5. Install dependencies first (Docker caches this layer, so rebuilds are faster when only your code changes)
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# 6. Copy in your application code
COPY . .

# 7. Start the web server
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8080"]