package com.allyis.timetracker.ui;


import android.content.Context;
import android.database.DataSetObserver;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.TextView;

import java.util.ArrayList;
import com.allyis.timetracker.domain.model.ClockPunch;
import com.allyis.timetracker.time_tracker.R;

import org.w3c.dom.Text;

public class MyCustomAdapter extends BaseExpandableListAdapter {

    private LayoutInflater inflater;
    private ArrayList<ClockPunch> mParent;

    public MyCustomAdapter(Context context, ArrayList<ClockPunch> parent){
        mParent = parent;
        inflater = LayoutInflater.from(context);
    }

    @Override
    //counts the number of group/parent items so the list knows how many times calls getGroupView() method
    public int getGroupCount() {
        return mParent.size();
    }

    @Override
    //counts the number of children items so the list knows how many times calls getChildView() method
    public int getChildrenCount(int i) {
        return mParent.get(i).getArrayChildren().size();
    }

    @Override
    //gets the title of each parent/group
    // X----- In terms of date, since drop down is based on each day ----X
    public Object getGroup(int i) {
        return mParent.get(i).geteventDate();
    }

    @Override
    //gets the name of each item
    public Object getChild(int i, int i1) {
        return mParent.get(i).getArrayChildren().get(i1);
    }

    @Override
    public long getGroupId(int i) {
        return i;
    }

    @Override
    public long getChildId(int i, int i1) {
        return i1;
    }

    @Override
    public boolean hasStableIds() {
        return true;
    }

    @Override
    //in this method you must set the text to see the parent/group on the list
    public View getGroupView(int groupPosition, boolean b, View view, ViewGroup viewGroup) {

        ViewHolder holder = new ViewHolder();

        if (view == null) {
            view = inflater.inflate(R.layout.list_parent, viewGroup, false);
            holder.groupTitleView = (TextView) view.findViewById(R.id.date);
            view.setTag(holder);
        }
        else{
            holder = (ViewHolder) view.getTag();
        }
        holder.groupTitleView.setText(mParent.get(groupPosition).geteventDate());

        //return the entire view
        return view;
    }

    @Override
    //in this method you must set the text to see the children on the list
    public View getChildView(int groupPosition, int childPosition, boolean isLastChild, View view, ViewGroup viewGroup) {

        ViewHolder holder = new ViewHolder();

        if (view == null) {
            view = inflater.inflate(R.layout.list_child, viewGroup, false);
        }
        TextView textView = (TextView) view.findViewById(R.id.time);
        textView.setText(mParent.get(groupPosition).getArrayChildren().get(childPosition));
        view.setTag(holder);

        //return the entire view
        return view;
    }

    @Override
    public boolean isChildSelectable(int i, int i1) {
        return true;
    }

    @Override
    public void registerDataSetObserver(DataSetObserver observer) {
        /* used to make the notifyDataSetChanged() method work */
        super.registerDataSetObserver(observer);
    }

// Intentionally put on comment, if you need on click deactivate it
/*  @Override
    public void onClick(View view) {
        ViewHolder holder = (ViewHolder)view.getTag();
        if (view.getId() == holder.button.getId()){

           // DO YOUR ACTION
        }
    }*/

    protected class ViewHolder {
        TextView groupTitleView;
    }
}