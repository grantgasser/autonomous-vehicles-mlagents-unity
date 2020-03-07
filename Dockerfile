FROM python:3.7.6-slim-buster

WORKDIR /usr/src/app

COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

COPY rl-training/vs03_02/ ./rl-training/
