package com.allyis.timetracker.domain.model;

import java.util.ArrayList;

public class ClockPunch {
    public int eventId;
    public String eventDate;
    public String eventTime;
    public String eventDescr;
    public String eventProject;

    private String mTitle;
    private ArrayList<String> mArrayChildren;

    public ClockPunch(int id, String date, String time, String descr){
        this.eventId = id;
        this.eventDate = date;
        this.eventTime = time;
        this.eventDescr = descr;
        //this.eventProject = project;
        }

public ClockPunch(){
        }

    //public String getTitle() { return mTitle; } ---replaced by getevenDate() since List Parent titles are based on dates

    //public void setTitle(String title) { mTitle = title; }

    public ArrayList<String> getArrayChildren() {
        return mArrayChildren;
    }

    public void setArrayChildren(ArrayList<String> arrayChildren) {
        mArrayChildren = arrayChildren;
    }


public int geteventId(){
        return eventId;
        }

public String geteventDate(){
        return eventDate;
        }

public String geteventTime(){
        return eventTime;
        }

public String geteventDescr(){
        return eventDescr;
        }

public String geteventProject(){
        return eventProject;
        }

public void seteventId(int eventId){
        this.eventId = eventId;
        }

public void seteventDate(String eventDate){
        this.eventDate = eventDate;
        }

public void seteventTime(String eventTime){this.eventTime = eventTime;}

public void seteventDescr(String eventDescr){
        this.eventDescr = eventDescr;
        }

public void seteventProject(String eventProject){
        this.eventProject = eventProject;
        }

        }

