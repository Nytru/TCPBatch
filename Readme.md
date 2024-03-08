how to run:
- run `docker compose up -d`
- attach to docker container via console `docker attach {container_id}`
- go to solution folder `cd TCP`
- run `chmod +x ./run.sh`
- run run.sh with arg1=output_folder arg2=solution_folder `./run.sh ./ ./`
- run server script `./run_server.sh --port 8080 --max-threads 150 --size 1000000 --path ./`
- attach new terminal to container `docker attach {container_id}`
- go to solution folder `cd TCP`
- run sender script `./run_client.sh log.txt 127.0.0.1:8080`
