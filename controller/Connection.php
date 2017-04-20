<?php
/**
 * Created by IntelliJ IDEA.
 * User: xSmoking
 * Date: 4/20/2017
 * Time: 4:52 AM
 */

class Connection
{
    private $mysqli;
    private $host = "127.0.0.1";
    private $user = "root";
    private $password = "2296agosto";
    private $database = "habbo";

    public function connection()
    {
        $this->mysqli = mysqli_connect($this->host, $this->user, $this->password, $this->database) or die(mysqli_error());
    }

    public function getConnection()
    {
        return $this->mysqli;
    }

}

$connectionFactory = new Connection();
$connectionFactory->connection();
$mysqli = $connectionFactory->getConnection();
$mysqli->set_charset("utf-8");