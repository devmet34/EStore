

function log(msg) {
  console.log(msg);

}

function changeProductsHtml(resp) {

  document.getElementById("products").innerHTML = resp;
  log("changeProductsHtml");
}

const getForgeryToken = () => {
  try {
    const forgeryToken = document.getElementsByName('__RequestVerificationToken')[0].value;
    return forgeryToken
  }
  catch (ex) {
    throw new Error("problem getting forgery token."+ex);
  }
}

function findProducts() {
  const uri = "findproducts";
  const find = document.getElementById("find").value;
  $.ajax(uri, {
    data: { productName: find }
  }).done(resp => {
    changeProductsHtml(resp);
  });
}
function sortProductsBy() {
  const uri = "sortproducts";
  const sort = $('#sortby').val();

  //sessionStorage.setItem("sortby", sort);
  $.ajax(uri,
    {
      data: { "sortBy": sort }
    }
  ).done((resp) => {
    changeProductsHtml(resp)
    //document.getElementById("products").innerHTML = res;
  });

  //document.getElementById("filterForm").submit();
}

function goNextPage() {
  const pageNumber = parseInt($('#pageN').text()) + 1
  goToPage(pageNumber)
}

function goPrevPage() {
  const page = parseInt($('#pageN').text()) - 1
  goToPage(page)
}
function goToPage(pageNumber) {
  const sort = $('#sortby').val();
  const find = $('#find').val();
  //const page = pageNumber //parseInt( $('#pageN').val())+ 1;
  const uri = "getproductsbypage";
 

  $.ajax(uri,
    {
      data: { page: pageNumber, sortBy: sort, find:find  }
    }
  ).done(resp => {
    changeProductsHtml(resp);
    changePageN(pageNumber);
    
  })
    
  
}

function changePageN(pageNumber) {
  let pageN = document.getElementById("pageN");
  pageN.textContent = pageNumber;
  if (pageNumber > 1)
    ActivatePrev();
  
  
}

function ActivatePrev() {
  
  const prev = document.getElementById("prevA");
  prev.hidden = false;
  
}

function getBasket() {

  const uri = "basket/getbasket"
  fetch(uri, { })
    .then(res => res.text())
    .then(r => {
      $('#basket').html(r);
      $('#basket').show();
      
    }

  );
      
}

function closeBasket() {
  $('#basket').hide();
}

 function updateBasketCount() {
  const uri = "basket/getbasketcount";
   //$('#basketToggle').text(' ' + basketCount)
 
   fetch(uri, {
     method: "get"

   }).then(r => r.text()).then(r =>
     $('#basketToggle').text(' '+r)
   );
   
 
}

function setBasketItem(productId,isFromBasket) {
  
  
  const qt = !isFromBasket ? document.getElementById(productId+"-qt").value
  :document.getElementById(productId).value ;
  const forgeryToken = getForgeryToken();  
  const baseUrl = document.URL;
  const uri="basket/setbasketitem"
  //const url = baseUrl + "addproduct";
  console.debug("test:{id}" + productId);
  
  $.ajax(uri, {

    method: "post",
    headers: { RequestVerificationToken: forgeryToken },
    data: { productId: productId, qt: qt }

  }
  ).done(() => {
    updateBasketCount();
    showToast(1);
  }).fail(showToast(0))
  

  
  //post(url, id);

}

async function removeBasketItem(productId) {
  const uri = "basket/removebasketitem";
  const forgeryToken = getForgeryToken();  

  const remove =
    await fetch(uri,
      {
        method: "post",
        headers: {
          "Content-Type": "application/json",
          "RequestVerificationToken": forgeryToken
        },

        body: JSON.stringify(productId)

      }
    );

  let resp = await remove;
  if (!resp.ok) {
    log("Error during removing product");
    showToast(0);
    return;
  }
  getBasket();
  decreaseBasketCount();
  showToast();

}

const toastFailHtml = `
  <div id="bsToast" class="toast fixed-top d-flex alert alert-danger justify-content-center align-items-center" role="alert" aria-live="assertive" aria-atomic="true">
    
      <div class="toast-body">
        FAILED
      </div>
      <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
   
  </div>
  `

const toastSuccessHtml = `
  <div id="bsToast" class="toast fixed-top d-flex alert alert-success justify-content-center align-items-center" role="alert" aria-live="assertive" aria-atomic="true">
    
      <div class="toast-body">
        SUCCESS
      </div>
      <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
   
  </div>
  `
function showToast(n) {
  
  if (n==0) {
    document.body.innerHTML += toastFailHtml;
  }
  else { document.body.innerHTML +=toastSuccessHtml }
  //setTimeout(null, 1000);
  const bsToastEl = document.getElementById("bsToast");
  let toastEl =  new bootstrap.Toast(bsToastEl);
  toastEl.show();
}



function decreaseBasketCount() {
  let basketToggle = $('#basketToggle');
  let basketCount = basketToggle.text();
  const newCount = parseInt(basketCount) - 1;
  basketToggle.text(' ' + newCount);
  //$('#basketToggle').text(' ' + basketCount)
}

function refreshBasket(html) {
  $('#basket').html(html);
}


function test() {
  const forgeryToken = getForgeryToken();
  const id = 3;
  const baseUrl = document.URL;
  const uri = "addproduct"
  const url = baseUrl + "addproduct";
  console.debug("test:{id}" + id);


}

/*
function increaseBasket() {
  let basketToggle = $('#basketToggle');
  let basketCount = basketToggle.text();
  const newCount = parseInt(basketCount)+1;
  basketToggle.text(' '+newCount);
  //$('#basketToggle').text(' ' + basketCount)
}

function decreaseBasket() {
  let basketToggle = $('#basketToggle');
  let basketCount = basketToggle.text();
  const newCount = parseInt(basketCount) - 1;
  basketToggle.text(' ' + newCount);
  //$('#basketToggle').text(' ' + basketCount)
}

function SubtractProduct(productId) {

  const forgeryToken = getForgeryToken();
  const baseUrl = document.URL;
  const uri = "addproduct"
  //const url = baseUrl + "addproduct";
  console.debug("test:{id}" + productId);

  $.ajax(uri, {

    method: "post",
    headers: { RequestVerificationToken: forgeryToken },
    data: { productId: productId }

  }
  );

  //post(url, id);

}

*/



