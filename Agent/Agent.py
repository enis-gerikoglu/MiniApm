import json
import socket
import time
from datetime import datetime, timezone


import psutil
import pika


def bytes_to_mb(value):
    return round(value / (1024 * 1024), 2)

def get_metrics():
    memory = psutil.virtual_memory()
    disk = psutil.disk_usage("/")

    return {
        "machineName": socket.gethostname(),
        "os": "macOS",

        "cpuUsage": round(psutil.cpu_percent(interval=1), 2),
        "cpuCoreCount": psutil.cpu_count(logical=True),

        "memoryUsage": round(memory.percent, 2),
        "totalMemory": bytes_to_mb(memory.total),
        "availableMemory": bytes_to_mb(memory.available),
        "usedMemory": bytes_to_mb(memory.used),

        "diskUsage": round(disk.percent, 2),
        "totalDisk": bytes_to_mb(disk.total),
        "freeDisk": bytes_to_mb(disk.free),
        "usedDisk": bytes_to_mb(disk.used),

        "timestamp": datetime.now(timezone.utc).isoformat()
    }

def create_rabbitmq_channel():
    credentials = pika.PlainCredentials("guest", "guest")
    connection = pika.BlockingConnection(
        pika.ConnectionParameters(
            host="localhost",
            port=5672,
            credentials=credentials
        )
    )

    channel = connection.channel()
    channel.exchange_declare(
        exchange="apm.metrics",
        exchange_type="topic",
        durable=True
    )

    return connection, channel



def main():
    connection, channel = create_rabbitmq_channel()

    try:
        with open("metrics.jsonl", "a") as f:
            while True:
                metrics = get_metrics()
                line = json.dumps(metrics)

                print(json.dumps(metrics, indent=2))
                f.write(line + "\n")
                f.flush()

                channel.basic_publish(
                    exchange="apm.metrics",
                    routing_key="metrics.mac",
                    body=line,
                    properties=pika.BasicProperties(
                        delivery_mode=2
                    )
                )

                time.sleep(5)

    finally:
        connection.close()

if __name__ == "__main__":
    main()