<?php
/**
 * Created by IntelliJ IDEA.
 * User: FAVELA
 * Date: 4/20/2017
 * Time: 4:56 AM
 */

require_once './Connection.php';
require_once './Functions.php';

class ajaxController
{
    function __construct()
    {
        if (isset($_POST['action']) AND method_exists($this, $_POST['action']))
        {
            call_user_func_array(array($this, $_POST['action']), func_get_args());
        }
    }

    public function RegisterUser()
    {
        global $mysqli;
        $functions = new Functions();
        $type = false;
        $username = $functions->FilterText($_POST['username']);
        $email = $functions->FilterText($_POST['email']);
        $pass = $functions->HoloHashMD5($_POST['password']);

        $userCheck = $mysqli->query("SELECT * FROM users WHERE username = '" . $username . "'") or die($mysqli->error);
        $emailCheck = $mysqli->query("SELECT * FROM users WHERE mail = '" . $email . "'") or die($mysqli->error);

        if (!$username || !$email || !$pass || !$_POST['password_2'])
        {
            $message = "Preencha todos os campos";
        }
        else if (strlen($username) < 4)
        {
            $message = "Nome de usuário deve possuir 4 ou mais caracteres";
        }
        else if (strlen($_POST['password']) < 8)
        {
            $message = "Senha deve possuir 8 ou mais caracteres";
        }
        else if ($_POST['password'] != $_POST['password_2'])
        {
            $message = "As senhas não coincidem";
        }
        else if ($userCheck->num_rows > 0)
        {
            $message = "Este nome de usuário já está sendo utilizado";
        }
        else if ($emailCheck->num_rows > 0)
        {
            $message = "Este endereço de e-mail já está sendo utilizado";
        }
        else
        {
            session_start();
            $mysqli->query("INSERT INTO users(`username`, `password`, `mail`, `account_created`, `last_online`, `ip_reg`, `ip_last`) VALUES(
                                    '" . $username . "',
                                    '" . $pass . "',
                                    '" . $email . "',
                                    '" . time() . "',
                                    '" . time() . "',
                                    '" . $_SERVER['REMOTE_ADDR'] . "',
                                    '" . $_SERVER['REMOTE_ADDR'] . "')") or die($mysqli->error);
            $userSql = $mysqli->query("SELECT * FROM users WHERE username = '" . $username . "'") or die($mysqli->error);
            $user = $userSql->fetch_assoc();
            $mysqli->query("INSERT INTO `user_stats` (id) VALUES ('" . $user['id'] . "')") or die($mysqli->error);
            $_SESSION['username'] = $username;
            $type = true;
            $message = "Cadastrado com sucesso";
        }

        echo json_encode(array(
            'message' => $message,
            'type'    => $type,
        ));
    }

    public function LoginUser()
    {
        global $mysqli;
        $functions = new Functions();
        $type = false;
        $username = $functions->FilterText($_POST['username']);
        $pass = $functions->HoloHashMD5($_POST['password']);
        $remindme = $_POST['remindme'];

        $check = $mysqli->query("SELECT * FROM users WHERE username = '" . $username . "' AND password = '" . $pass . "' OR mail = '" . $username . "' AND password = '" . $pass . "'") or die($mysqli->error);

        if (!$username || !$pass)
        {
            $message = "Preencha todos os campos";
        }
        else if ($check->num_rows == 0)
        {
            $message = "Usuário ou senha incorreto.";
        }
        else
        {
            $array = $check->fetch_assoc();
            $mysqli->query("UPDATE users SET ip_last = '" . $_SERVER['REMOTE_ADDR'] . "', last_online = '" . time() . "' WHERE username = '" . $array['username'] . "'") or die($mysqli->error);
            session_start();
            $_SESSION['username'] = $array['username'];
            if ($remindme == true)
            {
                setcookie('username', $array['username'], time() + 259200);
            }
            $type = true;
            $message = "Login efetuado com sucesso!";
        }

        echo json_encode(array(
            'message' => $message,
            'type'    => $type,
        ));
    }
}

$ajax = new ajaxController();
