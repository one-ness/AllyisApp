package com.allyis.timetracker.domain.access;

import java.util.ArrayList;

import android.database.Cursor;
import android.database.DatabaseUtils;
import android.database.sqlite.SQLiteOpenHelper;
import android.database.sqlite.SQLiteDatabase;
import android.content.ContentValues;
import android.content.Context;
import com.allyis.timetracker.domain.model.ClockPunch;

public class DatabaseHandler extends SQLiteOpenHelper {

    public static final String DATABASE_NAME = "eventsManager";
    public static final int DATABASE_VERSION = 1;
    public static final String TABLE_EVENTS = "events";

    //Columns of events table
    public static final String KEY_ID = "id";
    public static final String KEY_DATE = "date";
    public static final String KEY_TIME = "time";
    public static final String KEY_DESCRIPTION = "description";
    //public static final String KEY_PROJECT = "project";

    public DatabaseHandler(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        String CREATE_EVENTS_TABLE = "CREATE TABLE " + TABLE_EVENTS + "("
                      + KEY_ID + " INTEGER PRIMARY KEY," + KEY_DATE + " TEXT,"
                      + KEY_TIME + " TEXT," + KEY_DESCRIPTION + " TEXT" + ")";

        db.execSQL(CREATE_EVENTS_TABLE);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        //Drop older table if it exists
        db.execSQL("DROP TABLE IF EXISTS " + TABLE_EVENTS);
        //Creates Table again
        onCreate(db);
    }

    //Deletes all entries in the current table
    public void clearTable(){
        SQLiteDatabase db = this.getWritableDatabase();
        db.delete(TABLE_EVENTS, null, null);
    }

    public void addEvent(ClockPunch e){
        SQLiteDatabase db = this.getWritableDatabase();
        ContentValues values = new ContentValues();

        values.put("id", e.geteventId());
        values.put("date", e.geteventDate());
        values.put("time", e.geteventTime());
        values.put("description", e.geteventDescr());

        db.insert(TABLE_EVENTS, null, values);
        System.out.println("----EVENT ADDED---"); //Testing purpose - To check if user data is being stored correctly
        db.close();
    }

    public Cursor getData(Integer id) {
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor res = db.rawQuery("select * from users where id =" + id + "", null);
        return res;
    }

    public int numberOfRows() {
        SQLiteDatabase db = this.getReadableDatabase();
        int numRows = (int) DatabaseUtils.queryNumEntries(db, TABLE_EVENTS);
        return numRows;
    }

    public boolean updateEvents(Integer id, String date, String time, String description) {
        SQLiteDatabase db = this.getWritableDatabase();

        ContentValues values = new ContentValues();
        values.put(KEY_DATE, date);
        values.put(KEY_TIME, time);
        values.put(KEY_DESCRIPTION, description);

        //Log.d("id", String.valueOf(id));
        db.update("events", values, "id = ? ", new String[] { Integer.toString(id) } );
        return true;
    }

    //Get a Single Event
    public ClockPunch getEvent(int id) {
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor c = db.query(TABLE_EVENTS, new String[] {KEY_DATE, KEY_TIME, KEY_DESCRIPTION}, KEY_ID + "=?", new String[]{String.valueOf(id)}, null, null, null, null);

        if(c != null){
            c.moveToFirst();
            //add c.getInt(0) for id later on, after implementing id counter
            ClockPunch e = new ClockPunch(c.getInt(0), c.getString(1), c.getString(2), c.getString(3));
            return e;
        }
        return null;
    }

    //Get all events
    public ArrayList<ClockPunch> getAllEvents() {
        ArrayList<ClockPunch> event_list = new ArrayList<ClockPunch>();
        String selectQuery = "SELECT  * FROM " + TABLE_EVENTS;
        System.out.println("start"); //Testing reasons - mainly to check if view page is being accessed
        SQLiteDatabase db = this.getWritableDatabase();
        Cursor res = db.rawQuery(selectQuery, null);

        //Looping through all rows and adding to the list
        if(res.moveToFirst()){
            do{
                ClockPunch e = new ClockPunch();
                e.seteventId(res.getInt(0));
                e.seteventDate(res.getString(1));
                e.seteventTime(res.getString(2));
                e.seteventDescr(res.getString(3));
                event_list.add(e);
            }while(res.moveToNext());
        }
        res.close();
        return event_list;
    }

    //Getting count of how many events in database for the User
    public int getEventCount(){
        String countQuery = "SELECT  * FROM" + TABLE_EVENTS;
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor res = db.rawQuery(countQuery, null);
        int count  = res.getCount();
        res.close();
        return count;
    }

}
