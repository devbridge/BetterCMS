<script language="CS" runat="server"> 
void Page_Load(object sender, System.EventArgs e)
{      
    Response.StatusCode = 500;
} 
</script>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>OOPS ERROR OCCURED</title>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:800' rel='stylesheet' type='text/css' />
    <style type="text/css">
        .error-page {
            position: absolute;
            top: 0;
            left: 0;
            width: 40%;
            min-height: 100%;
            background: #2dacd4;
            cursor: default;
        }

        .error-block {
            position: absolute;
            top: 50%;
            right: -355px;
            width: 510px;
            height: 510px;
            margin-top: -255px;
            background: #fff;
            box-shadow: 6px 6px 0 0 rgba(0,0,0,0.1);
            font-family: 'Open Sans', sans-serif;
            font-weight: 800;
            text-transform: uppercase;
        }

        .error-number {
            position: absolute;
            left: 0;
            top: 0;
            width: 155px;
            height: 100%;
            background: #50badb;
            z-index: 2;
        }

            .error-number > span {
                display: block;
                width: 55px;
                line-height: 80px;
                padding: 70px 0 0 70px;
                color: #fff;
                font-size: 88px;
                text-shadow: 0 -1px 0 #219ac0;
                word-wrap: break-word;
            }

        .error-message {
            display: block;
            line-height: 1em;
            padding: 70px 0 0 185px;
            color: #2dacd4;
            font-size: 80px;
            text-shadow: 0 -1px 0 #219ac0;
            word-wrap: break-word;
        }

            .error-message span {
                display: block;
                line-height: 1.5em;
                font-size: 56px;
            }

        .go-back {
            position: absolute;
            left: 0;
            bottom: 0;
            width: 100%;
            height: 125px;
            padding: 55px 0 0 0;
            background: url('http://dve9d4b1d190c.cloudfront.net/images/error-bg.png');
        }

            .go-back span {
                margin: 0;
                color: #38393c;
                font-size: 53px;
                text-shadow: 0 -1px 0 #000;
                transition: color 0.2s ease-out;
                -moz-transition: color 0.2s ease-out;
                -webkit-transition: color 0.2s ease-out;
            }

        .back-button {
            position: relative;
            display: block;
            background: url('http://dve9d4b1d190c.cloudfront.net/images/error-back.png') no-repeat 80px 20px;
            padding-left: 185px;
            text-decoration: none;
            z-index: 3;
        }

            .back-button:hover {
                background-position: 80px -42px;
            }

                .back-button:hover span {
                    color: #d91d1d;
                    text-shadow: 0 -1px 0 #ac2020;
                }
    </style>
</head>
<body>
    <div class="error-page">
        <div class="error-block">
            <div class="error-number">
                <span>500</span>
            </div>
            <div class="error-message">
                Oops...
                Error
                <span>Occurred</span>
            </div>
            <div class="go-back">
                <a href="javascript:history.go(-1)" class="back-button"><span>Go Back?</span></a>
            </div>
        </div>
    </div>
</body>
</html>