﻿import moment from "moment"
import _ from "lodash"

export const defaultReportViewSettings = {
    projectId: 0,
    year: 0,
    week: 0,
    numberOfWeek: 4,
    numberOfWeekNotShowClosedItems: 2
};

const VueHelper = {
    valueTypeText: "Text",
    valueTypeNumber: "Number",
    valueTypeSelect: "Select",
    
    range: function(start: number, end: number){
        let array = [];
        for (let i = start; i<= end; i++){
            array.push(i);
        }

        return array;
    },
    formatMomentDate: function (dateString: string, formatString: string) {

        return moment(dateString).format(formatString);
    },

    getCurrentYearWeek: function(){
        return moment().isoWeekYear() * 100 + moment().isoWeek();
    },

    getLastYearWeek: function(yearweek: number){
        let year = this.calculateYear(yearweek);
        let week = this.calculateWeek(yearweek);
        let weekString = "" + week;
        if (week < 10)
            weekString = "0"+ week;
        let time = moment(`${year}W${weekString}`).subtract(1, "week");

        return time.isoWeekYear() * 100 + time.isoWeek();
    },
    
    getYearWeek: function(date: string): number{
        let week = moment(date).isoWeek();
        let year = moment(date).isoWeekYear();
        return this.calculateYearWeek(year, week);
    },
    
    getYearWeekDisplay: function(date: string): string{
        let week = moment(date).isoWeek();
        let year = moment(date).isoWeekYear();
        return `${week}-${year}`;
    },

    calculateYear: function (yearWeek: number) {
        return _.floor(yearWeek / 100, 0);
    },

    calculateWeek: function (yearWeek: number) {
        let year = _.floor(yearWeek / 100, 0);
        return yearWeek - year * 100;
    },

    calculateYearWeek: function(year: number, week: number){
        return year*100 + week;
    },

    getWorkingDays(yearWeek: number) {
        let year = this.calculateYear(yearWeek);
        let week = this.calculateWeek(yearWeek);

        let startDay = moment().isoWeekYear(year).isoWeek(week).isoWeekday("Monday");
        let endDay = moment().isoWeekYear(year).isoWeek(week).isoWeekday("Friday");

        return {startDay, endDay};
    },

    displayYearWeek: function (yearWeek: number) {
        let year = this.calculateYear(yearWeek);
        let week = this.calculateWeek(yearWeek);
        return `${year} - ${week}`;
    },

    formatYearWeek(yearWeek: number) {
        let year = this.calculateYear(yearWeek);
        let week = this.calculateWeek(yearWeek);
        return {text: `${week} - ${year}`, value: yearWeek}
    },

    createQueryStringDefault: function(projectId: number, yearweek: number){
        let viewSettings = defaultReportViewSettings;
        viewSettings.projectId = projectId;
        viewSettings.year = this.calculateYear(yearweek);
        viewSettings.week = this.calculateWeek(yearweek);

        return `projectId=${viewSettings.projectId}&year=${viewSettings.year}&week=${viewSettings.week}&numberOfWeek=${viewSettings.numberOfWeek}&numberOfWeekNotShowClosedItem=${viewSettings.numberOfWeekNotShowClosedItems}`
    },

    createQueryString: function(viewSettings: any) {
        return `projectId=${viewSettings.projectId}&year=${viewSettings.year}&week=${viewSettings.week}&numberOfWeek=${viewSettings.numberOfWeek}&numberOfWeekNotShowClosedItem=${viewSettings.numberOfWeekNotShowClosedItems}`
    },

    formatCurrentStatus: function(status: string) {
        if (status === "missed") return {color: "red", text: "Missed", tooltip: status};
        else if (status === "filled") return {color: "grey", text: "Filled", tooltip: status};
        else if (status === "ontime") return {color: "green", text: "On Time", tooltip: status};
        else if (status === "notfill") return {color: "yellow", text: "Not Fill", tooltip: status};
        else return undefined;
    },

    mustRequired(v: any) {
        if (v === 0) return true;
        return !!v || "Value is required";
    },

    mustBeNumber(v: any) {
        v = parseFloat(v);
        return !Number.isNaN(v) ? true : "Value must be number";
    },

    mustGreaterThanZero(v: number) {
        if (v < 0) return "Value must be greater than 0";
        return true;
    },

    requiredRules: [
        (v: any) => {
            if (v === 0) return true;
            return !!v || "Value is required";
        }
    ],
};

export default VueHelper;