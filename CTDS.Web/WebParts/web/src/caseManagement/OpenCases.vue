<template>
  <div>
    <appCustomSearch
      :searchObjects="casesAdvanceSearchObjects"
      @applyFilter="onApplyFilter"
    ></appCustomSearch>
    <div class="totalFoundText">{{ totalRows }} Cases found</div>
    <div>
      <b-table
        striped
        hover
        fixed
        :fields="fields"
         :no-sort-reset="true"
        :items="myProvider"
        :no-local-sorting="true"
        :current-page="pageNumber"
        :per-page="maxRowsPerPage"
        @sort-changed="onSortChange"
        @row-clicked="editCase"
        class="font-size-80"
      >
      </b-table>
    </div>
    <b-row>
      <b-col md="6" class="my-1">
        <b-pagination
          v-model="pageNumber"
          :total-rows="totalRows"
          :per-page="maxRowsPerPage"
          @change="getNewData"
          class="my-0"
        ></b-pagination>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import httpClient from "./../utils/httpRequestWrapper";
import caseTableFields from "./utils/caseTableFields.js";
import CaseStatusDropDown from "./CaseStatusDropDown.vue";
import CasePriorityDropDown from "./CasePriorityDropDown.vue";
import CustomSearch from "./../commonComponent/CustomSearch";
import casesAdvanceSearchObjects from "./utils/casesAdvanceSearchObjects.js";

export default {
  components: {
    appCaseStatusDropDown: CaseStatusDropDown,
    appCasePriorityDropDown: CasePriorityDropDown,
    appCustomSearch: CustomSearch
  },
  data() {
    return {
      sortBy:"CaseId",
      sortDesc:false,
      casesAdvanceSearchObjects,
      filters: null,
      openCases: [],
      pageNumber: 1,
      maxRowsPerPage: 5,
      totalRows: 0,
      fields: caseTableFields
    };
  },
  methods: {
    onSortChange(tableContext){
      this.pageNumber=1;
    this.sortBy=tableContext.sortBy;
    this.sortDesc=tableContext.sortDesc;
    },
    showModal() {
      this.$refs["my-modal"].show();
    },
    myProvider(ctx, callback) {
      const postObject={
        Queries: this.filters,
          MaxRowsPerPage: this.maxRowsPerPage,
          PageNumber: this.pageNumber,
          SortBy:this.sortBy,
          SortDesc:this.sortDesc
      };
      httpClient
        .post("/casewithquery",postObject)
        .then(response => {
          if (response.data === "token refreshed") {
            this.onApplyFilter(filters);
            return;
          }
          if (response.data.success === true) {
            const filteredCases = response.data.data.cases;
            this.openCases = [];
            let myOpenCases = [];
            for (let index = 0; index < filteredCases.length; ++index) {
              let obj = {
                CaseId: "KGH-19-" + filteredCases[index].caseId,
                Id: filteredCases[index].id,
                CreatedOn: this.convertDate(filteredCases[index].createdOn),
                Status: filteredCases[index].status,
                Description: filteredCases[index].description,
                Client: filteredCases[index].client,
                Notes: filteredCases[index].notes,
                Priority: filteredCases[index].priority
              };
              myOpenCases.push(obj);
            }
            this.openCases = myOpenCases;
            this.totalRows = response.data.data.totalCount;
            callback(this.openCases);
          } else {
            alert(response.data.message);
            callback([]);
          }
        })
        .catch(error => {
          console.log(error);
          callback([]);
        });
      return null;
    },
    onApplyFilter(filters) {
      this.filters = filters;
      if (this.pageNumber === 1) {
        this.pageNumber = 2;
        setTimeout(function() {
          this.pageNumber = 1;
        }, 2000);
      } else {
        this.pageNumber = 1;
      }
    },
    sortData(key) {
      console.log(key);
    },
    getNewData(val) {
      this.pageNumber = parseInt(val);
    },
    convertDate(someDate) {
      return new Date(someDate.match(/\d+/)[0] * 1).toString().substring(0, 16);
    },
    editCase: function(row) {
      const urlResource = `/casemanagement/${row.Id}`;
      httpClient
        .get(urlResource)
        .then(response => {
          if (response.data.success === true) {
            this.$store.dispatch("setCase", response.data.data);
            const url = `/case/${row.Id}`;
            this.$router.push(url);
          } else {
            console.log(response.data.data);
          }
        })
        .catch(error => {
          console.log(error);
        });
    }
  }
};
</script>
<style scoped>
@import url("./styles/openCasesStyle.css");
</style>
