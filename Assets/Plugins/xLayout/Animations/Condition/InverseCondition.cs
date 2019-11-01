namespace xLayout.Animations
{
    public class InverseCondition : UICondition
    {
        public UICondition[] originals;
        
        public override bool IsMet()
        {
            return !originals[0].IsMet();
        }
    }
}