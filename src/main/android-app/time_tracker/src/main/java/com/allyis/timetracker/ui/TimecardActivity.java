package com.allyis.timetracker.ui;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.Fragment;
import android.app.TimePickerDialog;
import android.content.ContentValues;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.TimeFormatException;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.TimePicker;
import android.widget.Toast;

import com.allyis.timetracker.domain.model.*;
import com.allyis.timetracker.domain.access.*;
import com.allyis.timetracker.time_tracker.R;

@SuppressWarnings( "deprecation" )
public class TimecardActivity extends Fragment implements View.OnClickListener {
    private EditText etDescription;
    private Button bDateChange, bTimeChange, bProjectChange, cancelButton, saveButton;
    private TextView tvDate, tvTime, tvProject;

    private int month;
    private int year;
    private int day;

    private int hourOne;
    private int minOne;

    private String descr;
    public static int id;

    private static DatabaseHandler dbHandler;
    ClockPunch clockPunch;

    private Calendar dateTime = Calendar.getInstance();
        private SimpleDateFormat sdf = new SimpleDateFormat("mm, dd, yyyy");

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.activity_timecard_input, container, false);

        //Calling Database
        dbHandler = new DatabaseHandler(getActivity());

        //Description editText
        etDescription = (EditText)view.findViewById(R.id.etDescription);

        // Set Buttons for Data Input (dateButton bDateChange, bTimeChange, bProject)
        bDateChange = (Button)view.findViewById(R.id.bDateChange);
        bTimeChange = (Button)view.findViewById(R.id.bTimeChange);
        bProjectChange = (Button)view.findViewById(R.id.bProjectChange);

        // Set Buttons OnClickListeners
        bDateChange.setOnClickListener(this);
        bTimeChange.setOnClickListener(this);
        bProjectChange.setOnClickListener(this);

        //Cancel & Save Buttons for the input data
        cancelButton = (Button)view.findViewById(R.id.cancelButton);
        cancelButton.setOnClickListener(this);
        saveButton = (Button)view.findViewById(R.id.saveButton);
        saveButton.setOnClickListener(this);

        //find textViews after pressing Button to input
        tvDate = (TextView)view.findViewById(R.id.tvDate);
        tvTime = (TextView)view.findViewById(R.id.tvTime);
        tvProject = (TextView)view.findViewById(R.id.tvProject);


        //Settings Intialization to check if user put in all details before saving clockPunch
        month = Settings.NOT_SET;
        year = Settings.NOT_SET;
        day = Settings.NOT_SET;
        hourOne = Settings.NOT_SET;
        minOne = Settings.NOT_SET;
        //project selection settings goes here (after api implementation)

    return view;
    }

    public static DatabaseHandler getDatabaseHandler() {
        return dbHandler;
    }

//Cancel Button Implementation. In case user wants to discard current details or go back to login screen
    private void cancel() {
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle("Cancel");

            builder.setMessage("Discard current event?");

        builder.setPositiveButton("Yes", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int id) {
                // User clicked Yes button
                if (getParentFragment() == null) {
                    getActivity().setResult(Activity.RESULT_CANCELED, new Intent());
                } else {
                    getParentFragment().getActivity().setResult(Activity.RESULT_CANCELED, new Intent());
                }

                getActivity().finish();
            }
        });
        builder.setNegativeButton("No", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int id) {
                // User cancelled the dialog
            }
        });
        AlertDialog dialog = builder.create();
        dialog.show();
    }

 @Override
public void onClick(View v){
     //Date selection in Save Tab
    if(v == bDateChange){
        final Calendar c = Calendar.getInstance();
        int day1 = c.get(Calendar.DAY_OF_MONTH);
        int month1 = c.get(Calendar.MONTH);
        int year2 = c.get(Calendar.YEAR);

        DatePickerDialog.OnDateSetListener theDateSetListener = new DatePickerDialog.OnDateSetListener(){
            @Override
            public void onDateSet(DatePicker view, int year1, int monthOfYear, int dayOfMonth){
                month = monthOfYear;
                day = dayOfMonth;
                year = year1;
                tvDate.setText(String.valueOf(month + 1) + "/" + String.valueOf(day) + "/" + String.valueOf(year));
                ContentValues values = new ContentValues();
                values.put("day", dayOfMonth);
                values.put("month", monthOfYear);
                values.put("year", year1);
            }
        };
        DatePickerDialog DateSelect = new DatePickerDialog(getActivity(), theDateSetListener, year2, month1, day1);
        DateSelect.setTitle("Set Date");
        DateSelect.show();

    }
    //Time selection in Save Tab
    else if(v == bTimeChange){
        final Calendar c = Calendar.getInstance();
        int hour, min;

        // set default time (8 hours, since it is the most common hours per day for a typical employee)
            hour = 8;  //c.get(Calendar.HOUR_OF_DAY);
            min = 0;   //c.get(Calendar.MINUTE);

        //Time Picker
         TimePickerDialog.OnTimeSetListener durationTimeListener = new TimePickerDialog.OnTimeSetListener(){
            @Override
            public void onTimeSet(TimePicker tp, int hourOfDay, int minute){
                tp.setIs24HourView(true);
                hourOne = hourOfDay;
                minOne = minute;
                tvTime.setText(String.valueOf(hourOne) + "hrs " + String.valueOf(minOne) + "mins");
            }
        };
        TimePickerDialog durationTimeSelect = new TimePickerDialog(getActivity(), durationTimeListener, hour, min, true);
        durationTimeSelect.setTitle("Set Duration Time");
        durationTimeSelect.show();
    }

    // For meanwhile a simple switch-case statement was used for project selection in Save Tab.
    //After client-server implementation project can be displayed in a similar table after obtaining project names according to the user
    else if(v == bProjectChange){
        String[] projectArray = {"Project 1", "Project 2", "Project 3"};
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle("Choose Project").setItems(projectArray, new DialogInterface.OnClickListener(){
            public void onClick(DialogInterface dialog, int which) {
                switch (which) {
                    case 0:
                        tvProject.setText("Project 1");
                        break;
                    case 1:
                        tvProject.setText("Project 2");
                        break;
                    case 2:
                        tvProject.setText("Project 3");
                        break;
                    default:
                        break;
                }
            }
        });
        //Shows the dialogue.. probably not needed. Just kept in case (for working conditions)
        AlertDialog VolumeSelect = builder.create();
        VolumeSelect.show();
    }
    else if( v == cancelButton){
        cancel();
    }
    else if( v == saveButton){
        if (	month == Settings.NOT_SET ||
                day == Settings.NOT_SET||
                year == Settings.NOT_SET	) {
            Toast.makeText(getActivity(), "Please select a date.", Toast.LENGTH_SHORT).show();
            return;
        }

        if (	hourOne == Settings.NOT_SET ||
                minOne == Settings.NOT_SET ) {
            Toast.makeText(getActivity(), "Please select a time duration.", Toast.LENGTH_SHORT).show();
            return;
        }

        //Description Bar
        descr = etDescription.getText().toString();
        //to make sure if user leaves it empty also to check if its accepting the user text
        if(descr.equals("") || descr.equals(null)){
            Toast.makeText(getActivity(), "Description not provided", Toast.LENGTH_SHORT).show();
            return;
        }

     //---------Obtaining data for storage-------------//

        //StringBuilder for Date
        StringBuilder sb = new StringBuilder();
        sb.append(month + 1).append("-").append(day).append("-").append(year);
        String date = sb.toString();

        //StringBuilder for Time
        StringBuilder sb2 = new StringBuilder();
        sb2.append(hourOne).append(":").append(minOne);
        String time = sb2.toString();

        //Saving user data in Database Handler
        clockPunch = new ClockPunch(id, date, time, descr);
        dbHandler.addEvent(clockPunch);
        dbHandler.close();
        id++;
        Toast.makeText(getActivity(), "Data has been Added!", Toast.LENGTH_LONG).show();
    }

}

    //BackButton same as canceled - implementation required if needed
    public void onBackPressed() {
        cancel();
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }
}
