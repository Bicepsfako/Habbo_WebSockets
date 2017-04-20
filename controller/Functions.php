<?php
/**
 * Created by IntelliJ IDEA.
 * User: FAVELA
 * Date: 4/20/2017
 * Time: 4:58 AM
 */

require_once './Connection.php';

class Functions
{

    public function FilterText($str, $advanced = false)
    {
        global $mysqli;
        if ($advanced == true)
        {
            return $mysqli->real_escape_string($str);
        }
        $str = $mysqli->real_escape_string(htmlspecialchars($str));
        return $str;
    }

    public function HoloHashMD5($password)
    {
        $hash_secret = "kasa%&(!kaskHAO)&!aksPL5645Sdsd54!&*(%";
        $string = md5($password . ($hash_secret));
        return $string;
    }

    public function codeGenerator($tamanho = 10, $maiusculas = true, $numeros = true, $simbolos = false)
    {
        $lmin = 'abcdefghijklmnopqrstuvwxyz';
        $lmai = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        $num = '1234567890';
        $simb = '!@#$%&*-()';
        $retorno = '';
        $caracteres = '';

        $caracteres .= $lmin;
        if ($maiusculas)
            $caracteres .= $lmai;
        if ($numeros)
            $caracteres .= $num;
        if ($simbolos)
            $caracteres .= $simb;

        $len = strlen($caracteres);
        for ($n = 1; $n <= $tamanho; $n++)
        {
            $rand = mt_rand(1, $len);
            $retorno .= $caracteres[$rand - 1];
        }
        return $retorno;
    }

    public function TextLimit($texto, $limite, $quebra = true)
    {
        $tamanho = strlen($texto);
        if ($tamanho <= $limite)
            $novo_texto = $texto;
        else
        {
            if ($quebra == true)
                $novo_texto = trim(substr($texto, 0, $limite)) . "...";
            else
            {
                $ultimo_espaco = strrpos(substr($texto, 0, $limite), " ");
                $novo_texto = trim(substr($texto, 0, $ultimo_espaco)) . "...";
            }
        }
        return $novo_texto;
    }

    public function GenerateTicket()
    {
        $data = "ST-";
        for ($i = 1; $i <= 6; $i++)
        {
            $data = $data . rand(0, 9);
        }
        $data = $data . "-";
        for ($i = 1; $i <= 20; $i++)
        {
            $data = $data . rand(0, 9);
        }
        $data = $data . "";
        $data = $data . rand(0, 5);
        return $data;
    }

    public function sendMusCommand($server_ip, $command, $data = NULL, $port = 30001)
    {
        $data = $command . chr(1) . $data;
        $connection = socket_create(AF_INET, SOCK_STREAM, getprotobyname('tcp'));
        socket_connect($connection, $server_ip, $port);
        if (!is_resource($connection))
        {
            socket_close($connection);
            return false;
        }
        else
        {
            socket_send($connection, $data, strlen($data), MSG_DONTROUTE);
            socket_close($connection);
            return true;
        }
    }

    function AtualPage($full = false)
    {
        $getPage = $_SERVER['PHP_SELF'];
        $page = end(explode("/", $getPage));
        if ($full == true)
            return $page;
        else
        {
            $page = explode(".", $page);
            return $page['0'];
        }
    }
}
