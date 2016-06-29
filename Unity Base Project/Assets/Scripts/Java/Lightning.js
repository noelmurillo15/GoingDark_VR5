var targetObject : GameObject;
var intensity : float = 0.4;
private var lineRenderer : LineRenderer;

function Start()
	{
	lineRenderer = GetComponent(LineRenderer);
	}

function Update ()
	{
	if(targetObject != null)
		{
		lineRenderer.SetPosition(0,this.transform.position);
		
		for(var i:int=1;i<4;i++)
			{
			var pos = Vector3.Lerp(this.transform.position, targetObject.transform.position, i/4.0f);
			
			pos.x += Random.Range(-intensity, intensity);
			pos.y += Random.Range(-intensity, intensity);
			
			lineRenderer.SetPosition(i,pos);
			}
		
		lineRenderer.SetPosition(i,targetObject.transform.position);
		}
	}