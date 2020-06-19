 <i-form :model="formMenuGroup" label-position="top" ref="formMenuGroup" :rules="ruleValidate" >
       
        <form-item :label="ml.rid">
                <i-input v-model="formMenuGroup.rid"></i-input>
        </form-item>
       
        <form-item :label="ml.displayorder">
                <i-input v-model="formMenuGroup.displayOrder"></i-input>
        </form-item>
       
        <form-item :label="ml.name1">
                <i-input v-model="formMenuGroup.name1"></i-input>
        </form-item>
       
        <form-item :label="ml.name2">
                <i-input v-model="formMenuGroup.name2"></i-input>
        </form-item>
       
        <form-item :label="ml.menugroupimage">
                <i-input v-model="formMenuGroup.menuGroupImage"></i-input>
        </form-item>
       
        <form-item :label="ml.description">
                <i-input v-model="formMenuGroup.description"></i-input>
        </form-item>
       
        <form-item :label="ml.web">
                <i-input v-model="formMenuGroup.web"></i-input>
        </form-item>
       
        <form-item :label="ml.name3">
                <i-input v-model="formMenuGroup.name3"></i-input>
        </form-item>
       
        <form-item :label="ml.name4">
                <i-input v-model="formMenuGroup.name4"></i-input>
        </form-item>
              
 </i-form>

 formMenuGroup: {

rid: '',

displayOrder: '',

name1: '',

name2: '',

menuGroupImage: '',

description: '',

web: '',

name3: '',

name4: '',

},

 
      public string rid{get;set;}
       
      public string displayOrder{get;set;}
       
      public string name1{get;set;}
       
      public string name2{get;set;}
       
      public string menuGroupImage{get;set;}
       
      public string description{get;set;}
       
      public string web{get;set;}
       
      public string name3{get;set;}
       
      public string name4{get;set;}
              

 ruleValidate: {

          rid: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          displayOrder: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name1: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name2: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuGroupImage: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          description: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          web: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name3: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name4: [
              { required: true, message: '*', trigger: 'blur' }
          ]
      
        },

public void Merge(MenuGroup entity)
        {

            this.rid = entity.Rid;     
  
            this.displayOrder = entity.DisplayOrder;     
  
            this.name1 = entity.Name1;     
  
            this.name2 = entity.Name2;     
  
            this.menuGroupImage = entity.MenuGroupImage;     
  
            this.description = entity.Description;     
  
            this.web = entity.Web;     
  
            this.name3 = entity.Name3;     
  
            this.name4 = entity.Name4;     
             
        }

  public void MapToEntity(MenuGroup entity)
        {

            entity.Rid = rid;
  
            entity.DisplayOrder = displayOrder;
  
            entity.Name1 = name1;
  
            entity.Name2 = name2;
  
            entity.MenuGroupImage = menuGroupImage;
  
            entity.Description = description;
  
            entity.Web = web;
  
            entity.Name3 = name3;
  
            entity.Name4 = name4;
       
        }