package com.allyis.timetracker.ui;

import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ExpandableListView;
import java.util.ArrayList;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;

import com.allyis.timetracker.time_tracker.R;
import com.allyis.timetracker.domain.model.ClockPunch;
import com.allyis.timetracker.domain.access.DatabaseHandler;

/*
    View tab/page implementation with data being displayed in an Expandable List View with categories(parent) organized by Dates
    and the sub-categories(children) containing Time & Description for the particular dates.
        -- (After API implementation, project sub-category can be added)--
 */

public class ListViewActivity extends Fragment {

    private static DatabaseHandler dbHandler;
    private ExpandableListView listview;

    ArrayList<ClockPunch> eventresults;
    MyCustomAdapter adapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.activity_load, container, false);

        //Database (implemented)
        dbHandler = new DatabaseHandler(getActivity());
        //For obtaining saved information from DatabaseHandler
        eventresults = dbHandler.getAllEvents();

        //List view display
        listview = (ExpandableListView) view.findViewById(R.id.lvData);


        ArrayList<String> arrayChildren; // = new ArrayList<String>();

        //Loop setup to add the time & description based on their date
            for (int i = 0; i < eventresults.size(); i++) {
                    arrayChildren = new ArrayList<String>();
                    arrayChildren.add(eventresults.get(i).geteventTime());
                    arrayChildren.add(eventresults.get(i).geteventDescr());
                    eventresults.get(i).setArrayChildren(arrayChildren);
            }
        //Output the result based on the adapter setup in MyCustomAdapter to view the Date inputs and their sub-categories Time & Description
            adapter = new MyCustomAdapter(getActivity(), eventresults);
            listview.setAdapter(adapter);


        listview.setOnItemClickListener(new OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> a, View v, int position, long id) {
                Object o = listview.getItemAtPosition(position);
            }
        });
        return view;

    }
}
