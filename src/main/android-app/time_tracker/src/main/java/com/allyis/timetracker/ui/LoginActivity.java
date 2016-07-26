package com.allyis.timetracker.ui;

import android.content.Intent;
import android.graphics.Color;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.allyis.timetracker.time_tracker.R;


public class LoginActivity extends ActionBarActivity {
    private EditText username = null;
    private EditText password = null;
    private TextView tries;
    private Button login;
    int count = 12; //12 tries given to user to input the correct login details (counter set up for that)

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login_page);
        username = (EditText) findViewById(R.id.userInput);
        password = (EditText) findViewById(R.id.passwordInput);
        tries = (TextView) findViewById(R.id.triesView);
        tries.setText(Integer.toString(count));
        login = (Button) findViewById(R.id.loginButton);
    }

    public void login (View view){
        if (username.getText().toString().equals("jai@allyis.com") &&
                password.getText().toString().equals("jai")) {
            Intent intent = new Intent(view.getContext(), MainActivity.class); //Connecting to input page when login is successful
            startActivity(intent);
        } else if(username.getText().toString().equals("shankar@allyis.com") &&
                password.getText().toString().equals("shankar")) {
            Intent intent = new Intent(view.getContext(), MainActivity.class);
            startActivity(intent);
        } else if(username.getText().toString().equals("coolguy@allyis.com") &&
                password.getText().toString().equals("coolguy")) {
            Intent intent = new Intent(view.getContext(), MainActivity.class);
            startActivity(intent);
        }else{
            Toast.makeText(getApplicationContext(), "Wrong Username or Password", Toast.LENGTH_SHORT).show();
            tries.setBackgroundColor(Color.RED);
            count--;
            tries.setText(Integer.toString(count)); //count reset if user get login info wrong
            if (count == 0) {
                login.setEnabled(false); //setEnabled disables text and doesn't allow any functioning of login button
            }
        }
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.login_page, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if(id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

}
